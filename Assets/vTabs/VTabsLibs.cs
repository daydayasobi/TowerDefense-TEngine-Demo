
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using UnityEditor;
using Type = System.Type;
using static VTabs.Libs.VUtils;



namespace VTabs.Libs
{

    public static class VUtils
    {

        #region Reflection


        public static object GetFieldValue(this object o, string fieldName)
        {
            var type = o as Type ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetFieldInfo(fieldName) is FieldInfo fieldInfo)
                return fieldInfo.GetValue(target);


            throw new System.Exception($"Field '{fieldName}' not found in type '{type.Name}' and its parent types");

        }
        public static object GetPropertyValue(this object o, string propertyName)
        {
            var type = o as Type ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetPropertyInfo(propertyName) is PropertyInfo propertyInfo)
                return propertyInfo.GetValue(target);


            throw new System.Exception($"Property '{propertyName}' not found in type '{type.Name}' and its parent types");

        }
        public static object GetMemberValue(this object o, string memberName)
        {
            var type = o as Type ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetFieldInfo(memberName) is FieldInfo fieldInfo)
                return fieldInfo.GetValue(target);

            if (type.GetPropertyInfo(memberName) is PropertyInfo propertyInfo)
                return propertyInfo.GetValue(target);


            throw new System.Exception($"Member '{memberName}' not found in type '{type.Name}' and its parent types");

        }

        public static void SetFieldValue(this object o, string fieldName, object value)
        {
            var type = o as Type ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetFieldInfo(fieldName) is FieldInfo fieldInfo)
                fieldInfo.SetValue(target, value);


            else throw new System.Exception($"Field '{fieldName}' not found in type '{type.Name}' and its parent types");

        }
        public static void SetPropertyValue(this object o, string propertyName, object value)
        {
            var type = o as Type ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetPropertyInfo(propertyName) is PropertyInfo propertyInfo)
                propertyInfo.SetValue(target, value);


            else throw new System.Exception($"Property '{propertyName}' not found in type '{type.Name}' and its parent types");

        }
        public static void SetMemberValue(this object o, string memberName, object value)
        {
            var type = o as Type ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetFieldInfo(memberName) is FieldInfo fieldInfo)
                fieldInfo.SetValue(target, value);

            else if (type.GetPropertyInfo(memberName) is PropertyInfo propertyInfo)
                propertyInfo.SetValue(target, value);


            else throw new System.Exception($"Member '{memberName}' not found in type '{type.Name}' and its parent types");

        }

        public static object InvokeMethod(this object o, string methodName, params object[] parameters) // todo handle null params (can't get their type)
        {
            var type = o as Type ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetMethodInfo(methodName, parameters.Select(r => r.GetType()).ToArray()) is MethodInfo methodInfo)
                return methodInfo.Invoke(target, parameters);


            throw new System.Exception($"Method '{methodName}' not found in type '{type.Name}', its parent types and interfaces");

        }


        public static T GetFieldValue<T>(this object o, string fieldName) => (T)o.GetFieldValue(fieldName);
        public static T GetPropertyValue<T>(this object o, string propertyName) => (T)o.GetPropertyValue(propertyName);
        public static T GetMemberValue<T>(this object o, string memberName) => (T)o.GetMemberValue(memberName);
        public static T InvokeMethod<T>(this object o, string methodName, params object[] parameters) => (T)o.InvokeMethod(methodName, parameters);




        public static FieldInfo GetFieldInfo(this Type type, string fieldName)
        {
            if (fieldInfoCache.TryGetValue(type, out var fieldInfosByNames))
                if (fieldInfosByNames.TryGetValue(fieldName, out var fieldInfo))
                    return fieldInfo;


            if (!fieldInfoCache.ContainsKey(type))
                fieldInfoCache[type] = new Dictionary<string, FieldInfo>();

            for (var curType = type; curType != null; curType = curType.BaseType)
                if (curType.GetField(fieldName, maxBindingFlags) is FieldInfo fieldInfo)
                    return fieldInfoCache[type][fieldName] = fieldInfo;


            return fieldInfoCache[type][fieldName] = null;

        }
        public static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
        {
            if (propertyInfoCache.TryGetValue(type, out var propertyInfosByNames))
                if (propertyInfosByNames.TryGetValue(propertyName, out var propertyInfo))
                    return propertyInfo;


            if (!propertyInfoCache.ContainsKey(type))
                propertyInfoCache[type] = new Dictionary<string, PropertyInfo>();

            for (var curType = type; curType != null; curType = curType.BaseType)
                if (curType.GetProperty(propertyName, maxBindingFlags) is PropertyInfo propertyInfo)
                    return propertyInfoCache[type][propertyName] = propertyInfo;


            return propertyInfoCache[type][propertyName] = null;

        }
        public static MethodInfo GetMethodInfo(this Type type, string methodName, params Type[] argumentTypes)
        {
            var methodHash = methodName.GetHashCode() ^ argumentTypes.Aggregate(0, (hash, r) => hash ^= r.GetHashCode());


            if (methodInfoCache.TryGetValue(type, out var methodInfosByHashes))
                if (methodInfosByHashes.TryGetValue(methodHash, out var methodInfo))
                    return methodInfo;



            if (!methodInfoCache.ContainsKey(type))
                methodInfoCache[type] = new Dictionary<int, MethodInfo>();

            for (var curType = type; curType != null; curType = curType.BaseType)
                if (curType.GetMethod(methodName, maxBindingFlags, null, argumentTypes, null) is MethodInfo methodInfo)
                    return methodInfoCache[type][methodHash] = methodInfo;

            foreach (var interfaceType in type.GetInterfaces())
                if (interfaceType.GetMethod(methodName, maxBindingFlags, null, argumentTypes, null) is MethodInfo methodInfo)
                    return methodInfoCache[type][methodHash] = methodInfo;



            return methodInfoCache[type][methodHash] = null;

        }

        static Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoCache = new();
        static Dictionary<Type, Dictionary<string, PropertyInfo>> propertyInfoCache = new();
        static Dictionary<Type, Dictionary<int, MethodInfo>> methodInfoCache = new();






        public static T GetCustomAttributeCached<T>(this MemberInfo memberInfo) where T : System.Attribute
        {
            if (!attributesCache.TryGetValue(memberInfo, out var attributes_byType))
                attributes_byType = attributesCache[memberInfo] = new();

            if (!attributes_byType.TryGetValue(typeof(T), out var attribute))
                attribute = attributes_byType[typeof(T)] = memberInfo.GetCustomAttribute<T>();

            return attribute as T;

        }

        static Dictionary<MemberInfo, Dictionary<Type, System.Attribute>> attributesCache = new();






        public static List<Type> GetSubclasses(this Type t) => t.Assembly.GetTypes().Where(type => type.IsSubclassOf(t)).ToList();

        public static object GetDefaultValue(this FieldInfo f, params object[] constructorVars) => f.GetValue(System.Activator.CreateInstance(((MemberInfo)f).ReflectedType, constructorVars));
        public static object GetDefaultValue(this FieldInfo f) => f.GetValue(System.Activator.CreateInstance(((MemberInfo)f).ReflectedType));

        public static IEnumerable<FieldInfo> GetFieldsWithoutBase(this Type t) => t.GetFields().Where(r => !t.BaseType.GetFields().Any(rr => rr.Name == r.Name));
        public static IEnumerable<PropertyInfo> GetPropertiesWithoutBase(this Type t) => t.GetProperties().Where(r => !t.BaseType.GetProperties().Any(rr => rr.Name == r.Name));


        public const BindingFlags maxBindingFlags = (BindingFlags)62;








        #endregion

        #region Collections


        public static T NextTo<T>(this IEnumerable<T> e, T to) => e.SkipWhile(r => !r.Equals(to)).Skip(1).FirstOrDefault();
        public static T PreviousTo<T>(this IEnumerable<T> e, T to) => e.Reverse().SkipWhile(r => !r.Equals(to)).Skip(1).FirstOrDefault();
        public static T NextToOrFirst<T>(this IEnumerable<T> e, T to) => e.NextTo(to) ?? e.First();
        public static T PreviousToOrLast<T>(this IEnumerable<T> e, T to) => e.PreviousTo(to) ?? e.Last();

        public static IEnumerable<T> InsertFirst<T>(this IEnumerable<T> ie, T t) => new[] { t }.Concat(ie);

        public static int IndexOfFirst<T>(this List<T> list, System.Func<T, bool> f) => list.FirstOrDefault(f) is T t ? list.IndexOf(t) : -1;
        public static int IndexOfLast<T>(this List<T> list, System.Func<T, bool> f) => list.LastOrDefault(f) is T t ? list.IndexOf(t) : -1;

        public static void SortBy<T, T2>(this List<T> list, System.Func<T, T2> keySelector) where T2 : System.IComparable => list.Sort((q, w) => keySelector(q).CompareTo(keySelector(w)));

        public static void RemoveValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            if (dictionary.FirstOrDefault(r => r.Value.Equals(value)) is var kvp)
                dictionary.Remove(kvp);
        }

        public static void ForEach<T>(this IEnumerable<T> sequence, System.Action<T> action) { foreach (T item in sequence) action(item); }



        public static T AddAt<T>(this List<T> l, T r, int i)
        {
            if (i < 0) i = 0;
            if (i >= l.Count)
                l.Add(r);
            else
                l.Insert(i, r);
            return r;
        }
        public static T RemoveLast<T>(this List<T> l)
        {
            if (!l.Any()) return default;

            var r = l.Last();

            l.RemoveAt(l.Count - 1);

            return r;
        }

        public static void Add<T>(this List<T> list, params T[] items)
        {
            foreach (var r in items)
                list.Add(r);
        }






        #endregion

        #region Math


        public static class MathUtil // MathUtils name is taken by UnityEditor.MathUtils 
        {

            public static float TriangleArea(Vector2 A, Vector2 B, Vector2 C) => Vector3.Cross(A - B, A - C).z.Abs() / 2;

            public static Vector2 LineIntersection(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
            {
                var a1 = B.y - A.y;
                var b1 = A.x - B.x;
                var c1 = a1 * A.x + b1 * A.y;

                var a2 = D.y - C.y;
                var b2 = C.x - D.x;
                var c2 = a2 * C.x + b2 * C.y;

                var d = a1 * b2 - a2 * b1;

                var x = (b2 * c1 - b1 * c2) / d;
                var y = (a1 * c2 - a2 * c1) / d;

                return new Vector2(x, y);

            }




            public static float Lerp(float f1, float f2, float t) => Mathf.LerpUnclamped(f1, f2, t);
            public static float Lerp(ref float f1, float f2, float t)
            {
                return f1 = Lerp(f1, f2, t);
            }

            public static Vector2 Lerp(Vector2 f1, Vector2 f2, float t) => Vector2.LerpUnclamped(f1, f2, t);
            public static Vector2 Lerp(ref Vector2 f1, Vector2 f2, float t)
            {
                return f1 = Lerp(f1, f2, t);
            }

            public static Vector3 Lerp(Vector3 f1, Vector3 f2, float t) => Vector3.LerpUnclamped(f1, f2, t);
            public static Vector3 Lerp(ref Vector3 f1, Vector3 f2, float t)
            {
                return f1 = Lerp(f1, f2, t);
            }

            public static Color Lerp(Color f1, Color f2, float t) => Color.LerpUnclamped(f1, f2, t);
            public static Color Lerp(ref Color f1, Color f2, float t)
            {
                return f1 = Lerp(f1, f2, t);
            }


            public static float Lerp(float current, float target, float speed, float deltaTime) => Mathf.Lerp(current, target, GetLerpT(speed, deltaTime));
            public static float Lerp(ref float current, float target, float speed, float deltaTime)
            {
                return current = Lerp(current, target, speed, deltaTime);
            }

            public static Vector2 Lerp(Vector2 current, Vector2 target, float speed, float deltaTime) => Vector2.Lerp(current, target, GetLerpT(speed, deltaTime));
            public static Vector2 Lerp(ref Vector2 current, Vector2 target, float speed, float deltaTime)
            {
                return current = Lerp(current, target, speed, deltaTime);
            }

            public static Vector3 Lerp(Vector3 current, Vector3 target, float speed, float deltaTime) => Vector3.Lerp(current, target, GetLerpT(speed, deltaTime));
            public static Vector3 Lerp(ref Vector3 current, Vector3 target, float speed, float deltaTime)
            {
                return current = Lerp(current, target, speed, deltaTime);
            }

            public static float SmoothDamp(float current, float target, float speed, ref float derivative, float deltaTime, float maxSpeed) => Mathf.SmoothDamp(current, target, ref derivative, .5f / speed, maxSpeed, deltaTime);
            public static float SmoothDamp(float current, float target, float speed, ref float derivative, float deltaTime)
            {
                return Mathf.SmoothDamp(current, target, ref derivative, .5f / speed, Mathf.Infinity, deltaTime);
            }
            public static float SmoothDamp(float current, float target, float speed, ref float derivative)
            {
                return SmoothDamp(current, target, speed, ref derivative, Time.deltaTime);
            }
            public static float SmoothDamp(ref float current, float target, float speed, ref float derivative, float deltaTime, float maxSpeed)
            {
                return current = SmoothDamp(current, target, speed, ref derivative, deltaTime, maxSpeed);
            }
            public static float SmoothDamp(ref float current, float target, float speed, ref float derivative, float deltaTime)
            {
                return current = SmoothDamp(current, target, speed, ref derivative, deltaTime);
            }
            public static float SmoothDamp(ref float current, float target, float speed, ref float derivative)
            {
                return current = SmoothDamp(current, target, speed, ref derivative, Time.deltaTime);
            }

            public static Vector2 SmoothDamp(Vector2 current, Vector2 target, float speed, ref Vector2 derivative, float deltaTime) => Vector2.SmoothDamp(current, target, ref derivative, .5f / speed, Mathf.Infinity, deltaTime);
            public static Vector2 SmoothDamp(Vector2 current, Vector2 target, float speed, ref Vector2 derivative)
            {
                return SmoothDamp(current, target, speed, ref derivative, Time.deltaTime);
            }
            public static Vector2 SmoothDamp(ref Vector2 current, Vector2 target, float speed, ref Vector2 derivative, float deltaTime)
            {
                return current = SmoothDamp(current, target, speed, ref derivative, deltaTime);
            }
            public static Vector2 SmoothDamp(ref Vector2 current, Vector2 target, float speed, ref Vector2 derivative)
            {
                return current = SmoothDamp(current, target, speed, ref derivative, Time.deltaTime);
            }

            public static Vector3 SmoothDamp(Vector3 current, Vector3 target, float speed, ref Vector3 derivative, float deltaTime) => Vector3.SmoothDamp(current, target, ref derivative, .5f / speed, Mathf.Infinity, deltaTime);
            public static Vector3 SmoothDamp(Vector3 current, Vector3 target, float speed, ref Vector3 derivative)
            {
                return SmoothDamp(current, target, speed, ref derivative, Time.deltaTime);
            }
            public static Vector3 SmoothDamp(ref Vector3 current, Vector3 target, float speed, ref Vector3 derivative, float deltaTime)
            {
                return current = SmoothDamp(current, target, speed, ref derivative, deltaTime);
            }
            public static Vector3 SmoothDamp(ref Vector3 current, Vector3 target, float speed, ref Vector3 derivative)
            {
                return current = SmoothDamp(current, target, speed, ref derivative, Time.deltaTime);
            }


            public static float GetLerpT(float lerpSpeed, float deltaTime) => 1 - Mathf.Exp(-lerpSpeed * 2f * deltaTime);
            public static float GetLerpT(float lerpSpeed)
            {
                return GetLerpT(lerpSpeed, Time.deltaTime);
            }



        }


        public static float DistanceTo(this float f1, float f2) => Mathf.Abs(f1 - f2);
        public static float DistanceTo(this Vector2 f1, Vector2 f2) => (f1 - f2).magnitude;
        public static float DistanceTo(this Vector3 f1, Vector3 f2) => (f1 - f2).magnitude;

        public static float Sign(this float f) => f == 0 ? 0 : Mathf.Sign(f);

        public static int Abs(this int f) => Mathf.Abs(f);
        public static float Abs(this float f) => Mathf.Abs(f);

        public static int Clamp(this int f, int f0, int f1) => Mathf.Clamp(f, f0, f1);
        public static float Clamp(this float f, float f0, float f1) => Mathf.Clamp(f, f0, f1);


        public static float Clamp01(this float f) => Mathf.Clamp(f, 0, 1);
        public static Vector2 Clamp01(this Vector2 f) => new(f.x.Clamp01(), f.y.Clamp01());
        public static Vector3 Clamp01(this Vector3 f) => new(f.x.Clamp01(), f.y.Clamp01(), f.z.Clamp01());


        public static int Pow(this int f, int pow) => (int)Mathf.Pow(f, pow);
        public static float Pow(this float f, float pow) => Mathf.Pow(f, pow);

        public static float Round(this float f) => Mathf.Round(f);
        public static float Ceil(this float f) => Mathf.Ceil(f);
        public static float Floor(this float f) => Mathf.Floor(f);

        public static int RoundToInt(this float f) => Mathf.RoundToInt(f);
        public static int CeilToInt(this float f) => Mathf.CeilToInt(f);
        public static int FloorToInt(this float f) => Mathf.FloorToInt(f);

        public static int ToInt(this float f) => (int)f;
        public static float ToFloat(this int f) => (float)f;
        public static float ToFloat(this double f) => (float)f;



        public static float Sqrt(this float f) => Mathf.Sqrt(f);

        public static int Max(this int f, int ff) => Mathf.Max(f, ff);
        public static int Min(this int f, int ff) => Mathf.Min(f, ff);
        public static float Max(this float f, float ff) => Mathf.Max(f, ff);
        public static float Min(this float f, float ff) => Mathf.Min(f, ff);

        public static float ClampMin(this float f, float limitMin) => Mathf.Max(f, limitMin);
        public static float ClampMax(this float f, float limitMax) => Mathf.Min(f, limitMax);


        public static float Loop(this float f, float boundMin, float boundMax)
        {
            while (f < boundMin) f += boundMax - boundMin;
            while (f > boundMax) f -= boundMax - boundMin;
            return f;
        }
        public static float Loop(this float f, float boundMax) => f.Loop(0, boundMax);

        public static float PingPong(this float f, float boundMin, float boundMax) => boundMin + Mathf.PingPong(f - boundMin, boundMax - boundMin);
        public static float PingPong(this float f, float boundMax) => f.PingPong(0, boundMax);


        public static float ProjectOn(this Vector2 v, Vector2 on) => Vector3.Project(v, on).magnitude;
        public static float ProjectOn(this Vector3 v, Vector3 on) => Vector3.Project(v, on).magnitude;

        public static float AngleTo(this Vector2 v, Vector2 to) => Vector2.Angle(v, to);

        public static Vector2 Rotate(this Vector2 v, float deg) => Quaternion.AngleAxis(deg, Vector3.forward) * v;

        public static float Smoothstep(this float f) { f = f.Clamp01(); return f * f * (3 - 2 * f); }

        public static float InverseLerp(this Vector2 v, Vector2 a, Vector2 b)
        {
            var ab = b - a;
            var av = v - a;
            return Vector2.Dot(av, ab) / Vector2.Dot(ab, ab);
        }


        public static bool IsOdd(this int i) => i % 2 == 1;
        public static bool IsEven(this int i) => i % 2 == 0;

        public static bool IsInRange(this int i, int a, int b) => i >= a && i <= b;
        public static bool IsInRange(this float i, float a, float b) => i >= a && i <= b;

        public static bool IsInRangeOf(this int i, IList list) => i.IsInRange(0, list.Count - 1);
        public static bool IsInRangeOf<T>(this int i, T[] array) => i.IsInRange(0, array.Length - 1);

        public static bool Approx(this float f1, float f2) => Mathf.Approximately(f1, f2);



        [System.Serializable]
        public class GaussianKernel
        {
            public static float[,] GenerateArray(int size, float sharpness = .5f)
            {
                float[,] kr = new float[size, size];

                if (size == 1) { kr[0, 0] = 1; return kr; }


                var sigma = 1f - Mathf.Pow(sharpness, .1f) * .99999f;
                var radius = (size / 2f).FloorToInt();


                var a = -2f * radius * radius / Mathf.Log(sigma);
                var sum = 0f;

                for (int y = 0; y < size; y++)
                    for (int x = 0; x < size; x++)
                    {
                        var rX = size % 2 == 1 ? (x - radius) : (x - radius) + .5f;
                        var rY = size % 2 == 1 ? (y - radius) : (y - radius) + .5f;
                        var dist = Mathf.Sqrt(rX * rX + rY * rY);
                        kr[x, y] = Mathf.Exp(-dist * dist / a);
                        sum += kr[x, y];
                    }

                for (int y = 0; y < size; y++)
                    for (int x = 0; x < size; x++)
                        kr[x, y] /= sum;

                return kr;
            }



            public GaussianKernel(bool isEvenSize = false, int radius = 7, float sharpness = .5f)
            {
                this.isEvenSize = isEvenSize;
                this.radius = radius;
                this.sharpness = sharpness;
            }

            public bool isEvenSize = false;
            public int radius = 7;
            public float sharpness = .5f;

            public int size => radius * 2 + (isEvenSize ? 0 : 1);
            public float sigma => 1 - Mathf.Pow(sharpness, .1f) * .99999f;

            public float[,] Array2d() // todo test and use GenerateArray
            {
                float[,] kr = new float[size, size];

                if (size == 1) { kr[0, 0] = 1; return kr; }

                var a = -2f * radius * radius / Mathf.Log(sigma);
                var sum = 0f;

                for (int y = 0; y < size; y++)
                    for (int x = 0; x < size; x++)
                    {
                        var rX = size % 2 == 1 ? (x - radius) : (x - radius) + .5f;
                        var rY = size % 2 == 1 ? (y - radius) : (y - radius) + .5f;
                        var dist = Mathf.Sqrt(rX * rX + rY * rY);
                        kr[x, y] = Mathf.Exp(-dist * dist / a);
                        sum += kr[x, y];
                    }

                for (int y = 0; y < size; y++)
                    for (int x = 0; x < size; x++)
                        kr[x, y] /= sum;

                return kr;
            }
            public float[] ArrayFlat()
            {
                var gk = Array2d();
                float[] flat = new float[size * size];

                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                        flat[(i * size + j)] = gk[i, j];

                return flat;
            }

        }







        #endregion

        #region Rects


        public static Rect Resize(this Rect rect, float px) { rect.x += px; rect.y += px; rect.width -= px * 2; rect.height -= px * 2; return rect; }

        public static Rect SetPos(this Rect rect, Vector2 v) => rect.SetPos(v.x, v.y);
        public static Rect SetPos(this Rect rect, float x, float y) { rect.x = x; rect.y = y; return rect; }

        public static Rect SetX(this Rect rect, float x) => rect.SetPos(x, rect.y);
        public static Rect SetY(this Rect rect, float y) => rect.SetPos(rect.x, y);
        public static Rect SetXMax(this Rect rect, float xMax) { rect.xMax = xMax; return rect; }
        public static Rect SetYMax(this Rect rect, float yMax) { rect.yMax = yMax; return rect; }

        public static Rect SetMidPos(this Rect r, Vector2 v) => r.SetPos(v).MoveX(-r.width / 2).MoveY(-r.height / 2);
        public static Rect SetMidPos(this Rect r, float x, float y) => r.SetMidPos(new Vector2(x, y));

        public static Rect Move(this Rect rect, Vector2 v) { rect.position += v; return rect; }
        public static Rect Move(this Rect rect, float x, float y) { rect.x += x; rect.y += y; return rect; }
        public static Rect MoveX(this Rect rect, float px) { rect.x += px; return rect; }
        public static Rect MoveY(this Rect rect, float px) { rect.y += px; return rect; }

        public static Rect SetWidth(this Rect rect, float f) { rect.width = f; return rect; }
        public static Rect SetWidthFromMid(this Rect rect, float px) { rect.x += rect.width / 2; rect.width = px; rect.x -= rect.width / 2; return rect; }
        public static Rect SetWidthFromRight(this Rect rect, float px) { rect.x += rect.width; rect.width = px; rect.x -= rect.width; return rect; }

        public static Rect SetHeight(this Rect rect, float f) { rect.height = f; return rect; }
        public static Rect SetHeightFromMid(this Rect rect, float px) { rect.y += rect.height / 2; rect.height = px; rect.y -= rect.height / 2; return rect; }
        public static Rect SetHeightFromBottom(this Rect rect, float px) { rect.y += rect.height; rect.height = px; rect.y -= rect.height; return rect; }

        public static Rect AddWidth(this Rect rect, float f) => rect.SetWidth(rect.width + f);
        public static Rect AddWidthFromMid(this Rect rect, float f) => rect.SetWidthFromMid(rect.width + f);
        public static Rect AddWidthFromRight(this Rect rect, float f) => rect.SetWidthFromRight(rect.width + f);

        public static Rect AddHeight(this Rect rect, float f) => rect.SetHeight(rect.height + f);
        public static Rect AddHeightFromMid(this Rect rect, float f) => rect.SetHeightFromMid(rect.height + f);
        public static Rect AddHeightFromBottom(this Rect rect, float f) => rect.SetHeightFromBottom(rect.height + f);

        public static Rect SetSize(this Rect rect, Vector2 v) => rect.SetWidth(v.x).SetHeight(v.y);
        public static Rect SetSize(this Rect rect, float w, float h) => rect.SetWidth(w).SetHeight(h);
        public static Rect SetSize(this Rect rect, float f) { rect.height = rect.width = f; return rect; }

        public static Rect SetSizeFromMid(this Rect r, Vector2 v) => r.Move(r.size / 2).SetSize(v).Move(-v / 2);
        public static Rect SetSizeFromMid(this Rect r, float x, float y) => r.SetSizeFromMid(new Vector2(x, y));
        public static Rect SetSizeFromMid(this Rect r, float f) => r.SetSizeFromMid(new Vector2(f, f));

        public static Rect AlignToPixelGrid(this Rect r) => GUIUtility.AlignRectToDevice(r);





        #endregion

        #region Colors


        public static Color Greyscale(float brightness, float alpha = 1) => new(brightness, brightness, brightness, alpha);

        public static Color SetAlpha(this Color color, float alpha) { color.a = alpha; return color; }
        public static Color MultiplyAlpha(this Color color, float k) { color.a *= k; return color; }





        #endregion

        #region Text


        public static bool IsEmpty(this string s) => s == "";
        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);

        public static bool IsLower(this char c) => System.Char.IsLower(c);
        public static bool IsUpper(this char c) => System.Char.IsUpper(c);
        public static bool IsDigit(this char c) => System.Char.IsDigit(c);
        public static bool IsLetter(this char c) => System.Char.IsLetter(c);
        public static bool IsWhitespace(this char c) => System.Char.IsWhiteSpace(c);

        public static char ToLower(this char c) => System.Char.ToLower(c);
        public static char ToUpper(this char c) => System.Char.ToUpper(c);



        public static string Decamelcase(this string s)
        {
            return Regex.Replace(Regex.Replace(s, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }
        public static string FormatVariableName(this string s, bool lowercaseFollowingWords = true)
        {
            return string.Join(" ", s.Decamelcase()
                         .Split(' ')
                         .Select(r => new[] { "", "and", "or", "with", "without", "by", "from" }.Contains(r.ToLower()) || (lowercaseFollowingWords && !s.Trim().StartsWith(r)) ? r.ToLower()
                                                                                                                                                                                 : r.Substring(0, 1).ToUpper() + r.Substring(1))).Trim(' ');
        }

        public static string Remove(this string s, string toRemove)
        {
            if (toRemove == "") return s;
            return s.Replace(toRemove, "");
        }






        #endregion

        #region Paths


        public static bool HasParentPath(this string path) => path.LastIndexOf('/') > 0;
        public static string GetParentPath(this string path) => path.HasParentPath() ? path.Substring(0, path.LastIndexOf('/')) : "";

        public static string GetFilename(this string path, bool withExtension = false) => withExtension ? Path.GetFileName(path) : Path.GetFileNameWithoutExtension(path);
        public static string GetExtension(this string path) => Path.GetExtension(path);


        public static string ToGlobalPath(this string localPath) => Application.dataPath + "/" + localPath.Substring(0, localPath.Length - 1);
        public static string ToLocalPath(this string globalPath) => "Assets" + globalPath.Replace(Application.dataPath, "");



        public static string CombinePath(this string p, string p2) => Path.Combine(p, p2);

        public static bool IsSubpathOf(this string path, string of) => path.StartsWith(of + "/") || of == "";

        public static string GetDirectory(this string pathOrDirectory)
        {
            var directory = pathOrDirectory.Contains('.') ? pathOrDirectory.Substring(0, pathOrDirectory.LastIndexOf('/')) : pathOrDirectory;

            if (directory.Contains('.'))
                directory = directory.Substring(0, directory.LastIndexOf('/'));

            return directory;

        }

        public static bool DirectoryExists(this string pathOrDirectory) => Directory.Exists(pathOrDirectory.GetDirectory());

        public static string EnsureDirExists(this string pathOrDirectory) // todo to EnsureDirectoryExists
        {
            var directory = pathOrDirectory.GetDirectory();

            if (directory.HasParentPath() && !Directory.Exists(directory.GetParentPath()))
                EnsureDirExists(directory.GetParentPath());

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return pathOrDirectory;

        }



        public static string ClearDir(this string dir)
        {
            if (!Directory.Exists(dir)) return dir;

            var diri = new DirectoryInfo(dir);
            foreach (var r in diri.EnumerateFiles()) r.Delete();
            foreach (var r in diri.EnumerateDirectories()) r.Delete(true);

            return dir;
        }






#if UNITY_EDITOR

        public static string EnsurePathIsUnique(this string path)
        {
            if (!path.DirectoryExists()) return path;

            var s = AssetDatabase.GenerateUniqueAssetPath(path); // returns empty if parent dir doesnt exist 

            return s == "" ? path : s;

        }

        public static void EnsureDirExistsAndRevealInFinder(string dir)
        {
            EnsureDirExists(dir);
            UnityEditor.EditorUtility.OpenWithDefaultApp(dir);
        }

#endif



        #endregion

        #region AssetDatabase

#if UNITY_EDITOR

        public static AssetImporter GetImporter(this Object t) => AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(t));

        public static string ToPath(this string guid) => AssetDatabase.GUIDToAssetPath(guid); // returns empty string if not found
        public static List<string> ToPaths(this IEnumerable<string> guids) => guids.Select(r => r.ToPath()).ToList();


        public static string ToGuid(this string pathInProject) => AssetDatabase.AssetPathToGUID(pathInProject);
        public static List<string> ToGuids(this IEnumerable<string> pathsInProject) => pathsInProject.Select(r => r.ToGuid()).ToList();

        public static string GetPath(this Object o) => AssetDatabase.GetAssetPath(o);
        public static string GetGuid(this Object o) => AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(o));

        public static string GetScriptPath(string scriptName) => AssetDatabase.FindAssets("t: script " + scriptName, null).FirstOrDefault()?.ToPath() ?? "scirpt not found"; // todonow to editorutils


        public static bool IsValidGuid(this string guid) => AssetDatabase.AssetPathToGUID(AssetDatabase.GUIDToAssetPath(guid), AssetPathToGUIDOptions.OnlyExistingAssets) != "";


        public static T Reimport<T>(this T t) where T : Object { AssetDatabase.ImportAsset(t.GetPath(), ImportAssetOptions.ForceUpdate); return t; }



        // toremove
        public static Object LoadGuid(this string guid) => AssetDatabase.LoadAssetAtPath(guid.ToPath(), typeof(Object));
        public static T LoadGuid<T>(this string guid) where T : Object => AssetDatabase.LoadAssetAtPath<T>(guid.ToPath());


        // toremove
        // public static List<string> FindAllAssetsOfType_guids(Type type) => AssetDatabase.FindAssets("t:" + type.Name).ToList();
        // public static List<string> FindAllAssetsOfType_guids(Type type, string path) => AssetDatabase.FindAssets("t:" + type.Name, new[] { path }).ToList();
        // public static List<T> FindAllAssetsOfType<T>() where T : Object => FindAllAssetsOfType_guids(typeof(T)).Select(r => (T)r.LoadGuid()).ToList();
        // public static List<T> FindAllAssetsOfType<T>(string path) where T : Object => FindAllAssetsOfType_guids(typeof(T), path).Select(r => (T)r.LoadGuid()).ToList();


#endif





        #endregion

        #region GlobalID

#if UNITY_EDITOR

        [System.Serializable]
        public struct GlobalID : System.IEquatable<GlobalID>
        {
            public Object GetObject() => GlobalObjectId.GlobalObjectIdentifierToObjectSlow(globalObjectId);
            public int GetObjectInstanceId() => GlobalObjectId.GlobalObjectIdentifierToInstanceIDSlow(globalObjectId);


            public int idType => globalObjectId.identifierType;
            public string guid => globalObjectId.assetGUID.ToString();
            public ulong fileId => globalObjectId.targetObjectId;
            public ulong prefabId => globalObjectId.targetPrefabId;

            public bool isNull => globalObjectId.identifierType == 0;
            public bool isAsset => globalObjectId.identifierType == 1;
            public bool isSceneObject => globalObjectId.identifierType == 2;

            public GlobalObjectId globalObjectId => _globalObjectId.Equals(default) && globalObjectIdString != null && GlobalObjectId.TryParse(globalObjectIdString, out var r) ? _globalObjectId = r : _globalObjectId;
            public GlobalObjectId _globalObjectId;

            public GlobalID(Object o) => globalObjectIdString = (_globalObjectId = GlobalObjectId.GetGlobalObjectIdSlow(o)).ToString();
            public GlobalID(string s) => globalObjectIdString = GlobalObjectId.TryParse(s, out _globalObjectId) ? s : s;

            public string globalObjectIdString;



            public bool Equals(GlobalID other) => this.globalObjectIdString.Equals(other.globalObjectIdString);

            public static bool operator ==(GlobalID a, GlobalID b) => a.Equals(b);
            public static bool operator !=(GlobalID a, GlobalID b) => !a.Equals(b);

            public override bool Equals(object other) => other is GlobalID otherglobalID && this.Equals(otherglobalID);
            public override int GetHashCode() => globalObjectIdString == null ? 0 : globalObjectIdString.GetHashCode();


            public override string ToString() => globalObjectIdString;




            public GlobalID UnpackForPrefab()
            {
                var unpackedFileId = (this.fileId ^ this.prefabId) & 0x7fffffffffffffff;

                var unpackedGId = new GlobalID($"GlobalObjectId_V1-{this.idType}-{this.guid}-{unpackedFileId}-0");

                return unpackedGId;

            }

        }

        public static GlobalID GetGlobalID(this Object o) => new(o);
        public static GlobalID[] GetGlobalIDs(this IEnumerable<int> instanceIds)
        {
            var unityGlobalIds = new GlobalObjectId[instanceIds.Count()];

            GlobalObjectId.GetGlobalObjectIdsSlow(instanceIds.ToArray(), unityGlobalIds);

            var globalIds = unityGlobalIds.Select(r => new GlobalID(r.ToString()));

            return globalIds.ToArray();

        }

        public static Object[] GetObjects(this IEnumerable<GlobalID> globalIDs)
        {
            var goids = globalIDs.Select(r => r.globalObjectId).ToArray();

            var objects = new Object[goids.Length];

            GlobalObjectId.GlobalObjectIdentifiersToObjectsSlow(goids, objects);

            return objects;

        }
        public static int[] GetObjectInstanceIds(this IEnumerable<GlobalID> globalIDs)
        {
            var goids = globalIDs.Select(r => r.globalObjectId).ToArray();

            var iids = new int[goids.Length];

            GlobalObjectId.GlobalObjectIdentifiersToInstanceIDsSlow(goids, iids);

            return iids;

        }


#endif




        #endregion

        #region Editor

#if UNITY_EDITOR


        public static class EditorPrefsCached
        {
            public static int GetInt(string key, int defaultValue = 0)
            {
                if (ints_byKey.ContainsKey(key))
                    return ints_byKey[key];
                else
                    return ints_byKey[key] = EditorPrefs.GetInt(key, defaultValue);

            }
            public static bool GetBool(string key, bool defaultValue = false)
            {
                if (bools_byKey.ContainsKey(key))
                    return bools_byKey[key];
                else
                    return bools_byKey[key] = EditorPrefs.GetBool(key, defaultValue);

            }
            public static float GetFloat(string key, float defaultValue = 0)
            {
                if (floats_byKey.ContainsKey(key))
                    return floats_byKey[key];
                else
                    return floats_byKey[key] = EditorPrefs.GetFloat(key, defaultValue);

            }
            public static string GetString(string key, string defaultValue = "")
            {
                if (strings_byKey.ContainsKey(key))
                    return strings_byKey[key];
                else
                    return strings_byKey[key] = EditorPrefs.GetString(key, defaultValue);

            }

            public static void SetInt(string key, int value)
            {
                ints_byKey[key] = value;

                EditorPrefs.SetInt(key, value);

            }
            public static void SetBool(string key, bool value)
            {
                bools_byKey[key] = value;

                EditorPrefs.SetBool(key, value);

            }
            public static void SetFloat(string key, float value)
            {
                floats_byKey[key] = value;

                EditorPrefs.SetFloat(key, value);

            }
            public static void SetString(string key, string value)
            {
                strings_byKey[key] = value;

                EditorPrefs.SetString(key, value);

            }


            static Dictionary<string, int> ints_byKey = new();
            static Dictionary<string, bool> bools_byKey = new();
            static Dictionary<string, float> floats_byKey = new();
            static Dictionary<string, string> strings_byKey = new();

        }

        public static class ProjectPrefs
        {
            public static int GetInt(string key, int defaultValue = 0) => EditorPrefsCached.GetInt(key + projectId, defaultValue);
            public static bool GetBool(string key, bool defaultValue = false) => EditorPrefsCached.GetBool(key + projectId, defaultValue);
            public static float GetFloat(string key, float defaultValue = 0) => EditorPrefsCached.GetFloat(key + projectId, defaultValue);
            public static string GetString(string key, string defaultValue = "") => EditorPrefsCached.GetString(key + projectId, defaultValue);

            public static void SetInt(string key, int value) => EditorPrefsCached.SetInt(key + projectId, value);
            public static void SetBool(string key, bool value) => EditorPrefsCached.SetBool(key + projectId, value);
            public static void SetFloat(string key, float value) => EditorPrefsCached.SetFloat(key + projectId, value);
            public static void SetString(string key, string value) => EditorPrefsCached.SetString(key + projectId, value);



            public static bool HasKey(string key) => EditorPrefs.HasKey(key + projectId);
            public static void DeleteKey(string key) => EditorPrefs.DeleteKey(key + projectId);



            public static int projectId => PlayerSettings.productGUID.GetHashCode();

        }



        public static void RecordUndo(this Object o, string operationName = "") => Undo.RecordObject(o, operationName);
        public static void Dirty(this Object o) => UnityEditor.EditorUtility.SetDirty(o);
        public static void Save(this Object o) => AssetDatabase.SaveAssetIfDirty(o);



        public static void SelectInInspector(this Object[] objects, bool frameInHierarchy = false, bool frameInProject = false)
        {
            void setHierarchyLocked(bool isLocked) => allHierarchies.ForEach(r => r?.GetMemberValue("m_SceneHierarchy")?.SetMemberValue("m_RectSelectInProgress", true));
            void setProjectLocked(bool isLocked) => allProjectBrowsers.ForEach(r => r?.SetMemberValue("m_InternalSelectionChange", isLocked));


            if (!frameInHierarchy) setHierarchyLocked(true);
            if (!frameInProject) setProjectLocked(true);

            Selection.objects = objects?.ToArray();

            if (!frameInHierarchy) EditorApplication.delayCall += () => setHierarchyLocked(false);
            if (!frameInProject) EditorApplication.delayCall += () => setProjectLocked(false);

        }
        public static void SelectInInspector(this Object obj, bool frameInHierarchy = false, bool frameInProject = false) => new[] { obj }.SelectInInspector(frameInHierarchy, frameInProject);

        static IEnumerable<EditorWindow> allHierarchies => _allHierarchies ??= typeof(Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow").GetFieldValue<IList>("s_SceneHierarchyWindows").Cast<EditorWindow>();
        static IEnumerable<EditorWindow> _allHierarchies;

        static IEnumerable<EditorWindow> allProjectBrowsers => _allProjectBrowsers ??= typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser").GetFieldValue<IList>("s_ProjectBrowsers").Cast<EditorWindow>();
        static IEnumerable<EditorWindow> _allProjectBrowsers;



        public static void MoveTo(this EditorWindow window, Vector2 position, bool ensureFitsOnScreen = true)
        {
            if (!ensureFitsOnScreen) { window.position = window.position.SetPos(position); return; }

            var windowRect = window.position;
            var unityWindowRect = EditorGUIUtility.GetMainWindowPosition();

            position.x = position.x.Max(unityWindowRect.position.x);
            position.y = position.y.Max(unityWindowRect.position.y);

            position.x = position.x.Min(unityWindowRect.xMax - windowRect.width);
            position.y = position.y.Min(unityWindowRect.yMax - windowRect.height);

            window.position = windowRect.SetPos(position);

        }



#endif

        #endregion

    }


    public static class VGUI
    {

        #region Drawing


        public static Rect Draw(this Rect rect, Color color)
        {
            EditorGUI.DrawRect(rect, color);

            return rect;

        }
        public static Rect Draw(this Rect rect) => rect.Draw(Color.black);

        public static Rect DrawOutline(this Rect rect, Color color, float thickness = 1)
        {

            rect.SetWidth(thickness).Draw(color);
            rect.SetWidthFromRight(thickness).Draw(color);

            rect.SetHeight(thickness).Draw(color);
            rect.SetHeightFromBottom(thickness).Draw(color);


            return rect;

        }
        public static Rect DrawOutline(this Rect rect, float thickness = 1) => rect.DrawOutline(Color.black, thickness);




        public static Rect DrawRounded(this Rect rect, Color color, int cornerRadius)
        {
            if (!curEvent.isRepaint) return rect;

            cornerRadius = cornerRadius.Min((rect.height / 2).FloorToInt()).Min((rect.width / 2).FloorToInt());

            if (cornerRadius < 0) return rect;

            GUIStyle style;

            void getStyle()
            {
                if (_roundedStylesByCornerRadius.TryGetValue(cornerRadius, out style)) return;

                var pixelsPerPoint = 2;

                var res = cornerRadius * 2 * pixelsPerPoint;
                var pixels = new Color[res * res];

                var white = Greyscale(1, 1);
                var clear = Greyscale(1, 0);
                var halfRes = res / 2;

                for (int x = 0; x < res; x++)
                    for (int y = 0; y < res; y++)
                    {
                        var sqrMagnitude = (new Vector2(x - halfRes + .5f, y - halfRes + .5f)).sqrMagnitude;
                        pixels[x + y * res] = sqrMagnitude <= halfRes * halfRes ? white : clear;
                    }

                var texture = new Texture2D(res, res);
                texture.SetPropertyValue("pixelsPerPoint", pixelsPerPoint);
                texture.hideFlags = HideFlags.DontSave;
                texture.SetPixels(pixels);
                texture.Apply();



                style = new GUIStyle();
                style.normal.background = texture;
                style.alignment = TextAnchor.MiddleCenter;
                style.border = new RectOffset(cornerRadius, cornerRadius, cornerRadius, cornerRadius);


                _roundedStylesByCornerRadius[cornerRadius] = style;

            }
            void draw()
            {
                SetGUIColor(color);

                style.Draw(rect, false, false, false, false);

                ResetGUIColor();

            }

            getStyle();
            draw();

            return rect;

        }
        public static Rect DrawRounded(this Rect rect, Color color, float cornerRadius) => rect.DrawRounded(color, cornerRadius.RoundToInt());

        static Dictionary<int, GUIStyle> _roundedStylesByCornerRadius = new();




        public static Rect DrawBlurred(this Rect rect, Color color, int blurRadius)
        {
            if (!curEvent.isRepaint) return rect;

            var pixelsPerPoint = .5f;
            // var pixelsPerPoint = 1f;

            var blurRadiusScaled = (blurRadius * pixelsPerPoint).RoundToInt().Max(1).Min(123);

            var croppedRectWidth = (rect.width * pixelsPerPoint).RoundToInt().Min(blurRadiusScaled * 2);
            var croppedRectHeight = (rect.height * pixelsPerPoint).RoundToInt().Min(blurRadiusScaled * 2);

            var textureWidth = croppedRectWidth + blurRadiusScaled * 2;
            var textureHeight = croppedRectHeight + blurRadiusScaled * 2;

            if (textureWidth <= 0 || textureWidth > 1232) return rect;
            if (textureHeight <= 0 || textureHeight > 1232) return rect;


            GUIStyle style;

            void getStyle()
            {
                if (_blurredStylesByTextureSize.TryGetValue((textureWidth, textureHeight), out style)) return;

                // VDebug.LogStart(blurRadius + "");

                var pixels = new Color[textureWidth * textureHeight];
                var kernel = GaussianKernel.GenerateArray(blurRadiusScaled * 2 + 1);

                for (int x = 0; x < textureWidth; x++)
                    for (int y = 0; y < textureHeight; y++)
                    {
                        var sum = 0f;

                        for (int xSample = (x - blurRadiusScaled).Max(blurRadiusScaled); xSample <= (x + blurRadiusScaled).Min(textureWidth - 1 - blurRadiusScaled); xSample++)
                            for (int ySample = (y - blurRadiusScaled).Max(blurRadiusScaled); ySample <= (y + blurRadiusScaled).Min(textureHeight - 1 - blurRadiusScaled); ySample++)
                                sum += kernel[blurRadiusScaled + xSample - x, blurRadiusScaled + ySample - y];

                        pixels[x + y * textureWidth] = Greyscale(1, sum);

                    }

                var texture = new Texture2D(textureWidth, textureHeight);
                texture.SetPropertyValue("pixelsPerPoint", pixelsPerPoint);
                texture.hideFlags = HideFlags.DontSave;
                texture.SetPixels(pixels);
                texture.Apply();


                style = new GUIStyle();
                style.normal.background = texture;
                style.alignment = TextAnchor.MiddleCenter;

                var borderX = ((textureWidth / 2f - 1) / pixelsPerPoint).FloorToInt();
                var borderY = ((textureHeight / 2f - 1) / pixelsPerPoint).FloorToInt();
                style.border = new RectOffset(borderX, borderX, borderY, borderY);

                _blurredStylesByTextureSize[(textureWidth, textureHeight)] = style;

                // VDebug.LogFinish();

            }
            void draw()
            {
                SetGUIColor(color);

                style.Draw(rect.SetSizeFromMid(rect.width + blurRadius * 2, rect.height + blurRadius * 2), false, false, false, false);

                ResetGUIColor();

            }

            getStyle();
            draw();

            return rect;

        }
        public static Rect DrawBlurred(this Rect rect, Color color, float blurRadius) => rect.DrawBlurred(color, blurRadius.RoundToInt());

        static Dictionary<(int, int), GUIStyle> _blurredStylesByTextureSize = new();




        static void DrawCurtain(this Rect rect, Color color, int dir)
        {
            void genTextures()
            {
                if (_gradientTextures != null) return;

                _gradientTextures = new Texture2D[4];

                // var pixels = Enumerable.Range(0, 256).Select(r => Greyscale(1, r / 255f));
                var pixels = Enumerable.Range(0, 256).Select(r => Greyscale(1, (r / 255f).Smoothstep()));

                var up = new Texture2D(1, 256);
                up.SetPixels(pixels.Reverse().ToArray());
                up.Apply();
                up.hideFlags = HideFlags.DontSave;
                up.wrapMode = TextureWrapMode.Clamp;
                _gradientTextures[0] = up;

                var down = new Texture2D(1, 256);
                down.SetPixels(pixels.ToArray());
                down.Apply();
                down.hideFlags = HideFlags.DontSave;
                down.wrapMode = TextureWrapMode.Clamp;
                _gradientTextures[1] = down;

                var left = new Texture2D(256, 1);
                left.SetPixels(pixels.ToArray());
                left.Apply();
                left.hideFlags = HideFlags.DontSave;
                left.wrapMode = TextureWrapMode.Clamp;
                _gradientTextures[2] = left;

                var right = new Texture2D(256, 1);
                right.SetPixels(pixels.Reverse().ToArray());
                right.Apply();
                right.hideFlags = HideFlags.DontSave;
                right.wrapMode = TextureWrapMode.Clamp;
                _gradientTextures[3] = right;

            }
            void draw()
            {
                SetGUIColor(color);

                GUI.DrawTexture(rect, _gradientTextures[dir]);

                ResetGUIColor();

            }

            genTextures();
            draw();

        }

        static Texture2D[] _gradientTextures;

        public static void DrawCurtainUp(this Rect rect, Color color) => rect.DrawCurtain(color, 0);
        public static void DrawCurtainDown(this Rect rect, Color color) => rect.DrawCurtain(color, 1);
        public static void DrawCurtainLeft(this Rect rect, Color color) => rect.DrawCurtain(color, 2);
        public static void DrawCurtainRight(this Rect rect, Color color) => rect.DrawCurtain(color, 3);






        #endregion

        #region Events


        public class WrappedEvent
        {
            public Event e;

            public bool isRepaint => e.type == EventType.Repaint;
            public bool isLayout => e.type == EventType.Layout;
            public bool isUsed => e.type == EventType.Used;
            public bool isMouseLeaveWindow => e.type == EventType.MouseLeaveWindow;
            public bool isMouseEnterWindow => e.type == EventType.MouseEnterWindow;
            public bool isContextClick => e.type == EventType.ContextClick;
            public bool isIgnore => e.type == EventType.Ignore;

            public bool isKeyDown => e.type == EventType.KeyDown;
            public bool isKeyUp => e.type == EventType.KeyUp;
            public KeyCode keyCode => e.keyCode;
            public char characted => e.character;

            public bool isExecuteCommand => e.type == EventType.ExecuteCommand;
            public string commandName => e.commandName;

            public bool isMouse => e.isMouse;
            public bool isMouseDown => e.type == EventType.MouseDown;
            public bool isMouseUp => e.type == EventType.MouseUp;
            public bool isMouseDrag => e.type == EventType.MouseDrag;
            public bool isMouseMove => e.type == EventType.MouseMove;
            public bool isScroll => e.type == EventType.ScrollWheel;
            public int mouseButton => e.button;
            public int clickCount => e.clickCount;
            public Vector2 mousePosition => e.mousePosition;
            public Vector2 mousePosition_screenSpace => GUIUtility.GUIToScreenPoint(e.mousePosition);
            public Vector2 mouseDelta => e.delta;

            public bool isDragUpdate => e.type == EventType.DragUpdated;
            public bool isDragPerform => e.type == EventType.DragPerform;
            public bool isDragExit => e.type == EventType.DragExited;

            public EventModifiers modifiers => e.modifiers;
            public bool holdingAnyModifierKey => modifiers != EventModifiers.None;

            public bool holdingAlt => e.alt;
            public bool holdingShift => e.shift;
            public bool holdingCtrl => e.control;
            public bool holdingCmd => e.command;
            public bool holdingCmdOrCtrl => e.command || e.control;

            public bool holdingAltOnly => e.modifiers == EventModifiers.Alt;        // in some sessions FunctionKey is always pressed?
            public bool holdingShiftOnly => e.modifiers == EventModifiers.Shift;        // in some sessions FunctionKey is always pressed?
            public bool holdingCtrlOnly => e.modifiers == EventModifiers.Control;
            public bool holdingCmdOnly => e.modifiers == EventModifiers.Command;
            public bool holdingCmdOrCtrlOnly => (e.modifiers == EventModifiers.Command || e.modifiers == EventModifiers.Control);

            public EventType type => e.type;

            public void Use() => e?.Use();


            public WrappedEvent(Event e) => this.e = e;

            public override string ToString() => e.ToString();

        }

        public static WrappedEvent Wrap(this Event e) => new(e);

        public static WrappedEvent curEvent => _curEvent ??= typeof(Event).GetFieldValue<Event>("s_Current").Wrap();
        static WrappedEvent _curEvent;





        #endregion

        #region Shortcuts


        public static Rect lastRect => GUILayoutUtility.GetLastRect();

        public static bool isDarkTheme => EditorGUIUtility.isProSkin;

        public static bool IsHovered(this Rect r) => r.Contains(curEvent.mousePosition);

        public static float GetLabelWidth(this string s) => GUI.skin.label.CalcSize(new GUIContent(s)).x;
        public static float GetLabelWidth(this string s, int fontSize)
        {
            SetLabelFontSize(fontSize);

            var r = s.GetLabelWidth();

            ResetLabelStyle();

            return r;

        }
        public static float GetLabelWidth(this string s, bool isBold)
        {
            if (isBold)
                SetLabelBold();

            var r = s.GetLabelWidth();

            if (isBold)
                ResetLabelStyle();

            return r;

        }
        public static float GetLabelWidth(this string s, int fontSize, bool isBold)
        {
            if (isBold)
                SetLabelBold();

            SetLabelFontSize(fontSize);

            var r = s.GetLabelWidth();

            ResetLabelStyle();

            return r;

        }

        public static void SetGUIEnabled(bool enabled) { _prevGuiEnabled = GUI.enabled; GUI.enabled = enabled; }
        public static void ResetGUIEnabled() => GUI.enabled = _prevGuiEnabled;
        static bool _prevGuiEnabled = true;

        public static void SetLabelFontSize(int size) => GUI.skin.label.fontSize = size;
        public static void SetLabelBold() => GUI.skin.label.fontStyle = FontStyle.Bold;
        public static void SetLabelAlignmentCenter() => GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        public static void ResetLabelStyle()
        {
            GUI.skin.label.fontSize = 0;
            GUI.skin.label.fontStyle = FontStyle.Normal;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUI.skin.label.wordWrap = false;
        }


        public static void SetGUIColor(Color c)
        {
            _guiColorStack.Push(GUI.color);

            GUI.color *= c;

        }
        public static void ResetGUIColor()
        {
            GUI.color = _guiColorStack.Pop();
        }

        static Stack<Color> _guiColorStack = new();



        public static float editorDeltaTime = .0166f;

        static void EditorDeltaTime_Update()
        {
            editorDeltaTime = (float)(EditorApplication.timeSinceStartup - _lastUpdateTime);

            _lastUpdateTime = EditorApplication.timeSinceStartup;

        }
        static double _lastUpdateTime;

        [InitializeOnLoadMethod]
        static void EditorDeltaTime_Subscribe()
        {
            EditorApplication.update -= EditorDeltaTime_Update;
            EditorApplication.update += EditorDeltaTime_Update;
        }




        #endregion

        #region Controls


        public static bool IconButton(Rect rect, string iconName, float iconSize = default, Color color = default, Color colorHovered = default, Color colorPressed = default)
        {
            var id = EditorGUIUtility.GUIToScreenRect(rect).GetHashCode();// GUIUtility.GetControlID(FocusType.Passive, rect);
            var isPressed = id == _pressedIconButtonId;

            var wasActivated = false;

            void icon()
            {
                if (!curEvent.isRepaint) return;


                if (color == default)
                    color = Color.white;

                if (colorHovered == default)
                    colorHovered = Color.white;

                if (colorPressed == default)
                    colorPressed = Color.white.SetAlpha(.6f);


                if (rect.IsHovered())
                    color = colorHovered;

                if (isPressed)
                    color = colorPressed;


                if (iconSize == default)
                    iconSize = rect.width.Min(rect.height);

                var iconRect = rect.SetSizeFromMid(iconSize);



                SetGUIColor(color);

                GUI.DrawTexture(iconRect, EditorIcons.GetIcon(iconName));

                ResetGUIColor();


            }
            void mouseDown()
            {
                if (!curEvent.isMouseDown) return;
                if (!rect.IsHovered()) return;

                _pressedIconButtonId = id;

                curEvent.Use();

            }
            void mouseUp()
            {
                if (!curEvent.isMouseUp) return;
                if (!isPressed) return;

                _pressedIconButtonId = 0;

                if (rect.IsHovered())
                    wasActivated = true;

                curEvent.Use();

            }
            void mouseDrag()
            {
                if (!curEvent.isMouseDrag) return;
                if (!isPressed) return;

                curEvent.Use();

            }

            rect.MarkInteractive();

            icon();
            mouseDown();
            mouseUp();
            mouseDrag();

            return wasActivated;

        }

        static int _pressedIconButtonId;




        #endregion

        #region Layout


        public static void Space(float px = 6) => GUILayout.Space(px);

        public static Rect ExpandWidthLabelRect() { GUILayout.Label(""/* , GUILayout.Height(0) */, GUILayout.ExpandWidth(true)); return lastRect; }
        public static Rect ExpandWidthLabelRect(float height) { GUILayout.Label("", GUILayout.Height(height), GUILayout.ExpandWidth(true)); return lastRect; }




        #endregion

        #region GUIColors


        public static class GUIColors
        {
            public static Color windowBackground => isDarkTheme ? Greyscale(.22f) : Greyscale(.78f); // prev backgroundCol
            public static Color pressedButtonBackground => isDarkTheme ? new Color(.48f, .76f, 1f, 1f) * 1.4f : new Color(.48f, .7f, 1f, 1f) * 1.2f; // prev pressedButtonCol
            public static Color greyedOutTint => Greyscale(.7f);
            public static Color selectedBackground => isDarkTheme ? new Color(.17f, .365f, .535f) : new Color(.2f, .375f, .555f) * 1.2f;
        }




        #endregion

        #region EditorIcons


        public static partial class EditorIcons
        {
            public static Texture2D GetIcon(string iconNameOrPath, bool returnNullIfNotFound = false)
            {
                iconNameOrPath ??= "";

                if (icons_byName.TryGetValue(iconNameOrPath, out var cachedResult) && cachedResult) return cachedResult;


                Texture2D icon = null;

                void getCustom()
                {
                    if (icon) return;
                    if (!customIcons.ContainsKey(iconNameOrPath)) return;

                    var pngBytesString = customIcons[iconNameOrPath];
                    var pngBytes = pngBytesString.Split("-").Select(r => System.Convert.ToByte(r, 16)).ToArray();

                    icon = new Texture2D(1, 1);

                    icon.LoadImage(pngBytes);

                }
                void getBuiltin()
                {
                    if (icon) return;

                    icon = typeof(EditorGUIUtility).InvokeMethod<Texture2D>("LoadIcon", iconNameOrPath) as Texture2D;

                }

                getCustom();
                getBuiltin();

                icons_byName[iconNameOrPath] = icon;

                if (icon == null && !returnNullIfNotFound) return Texture2D.grayTexture;
                else return icon;

            }

            static Dictionary<string, Texture2D> icons_byName = new();


            static Dictionary<string, string> customIcons = new()
            {
                ["Cross"] = "89-50-4E-47-0D-0A-1A-0A-00-00-00-0D-49-48-44-52-00-00-00-20-00-00-00-20-08-06-00-00-00-73-7A-7A-F4-00-00-00-09-70-48-59-73-00-00-0B-13-00-00-0B-13-01-00-9A-9C-18-00-00-00-01-73-52-47-42-00-AE-CE-1C-E9-00-00-00-04-67-41-4D-41-00-00-B1-8F-0B-FC-61-05-00-00-00-C5-49-44-41-54-78-01-ED-96-D1-0D-83-30-0C-44-9D-4E-D0-51-BA-02-13-B5-23-A4-1B-A4-13-31-42-3B-4A-37-70-8D-6A-04-42-E0-D8-88-E0-1F-3F-29-8A-50-1C-DF-05-48-62-80-20-08-9C-49-D2-20-22-5E-A9-BB-53-1B-FA-67-4A-E9-0B-0A-66-F3-06-5E-DA-79-6B-89-32-4E-BC-39-71-55-9C-63-47-B2-14-7F-01-3D-37-6A-BD-64-82-C7-7A-8E-1D-A9-9A-06-29-E1-62-35-9B-6F-C2-12-7B-B8-89-66-E2-1A-81-E6-E2-0A-13-ED-C5-2B-26-CE-11-57-98-D8-25-6E-D9-86-FE-B8-7E-02-D7-9F-10-3D-B7-21-7A-1E-44-96-C4-4D-4C-D0-E4-62-49-B8-61-22-4B-1A-B5-6D-38-BF-C7-3F-D4-3A-E9-6E-E7-B1-8E-63-55-68-0A-92-07-3F-16-63-41-92-E1-BF-80-B2-BB-20-09-82-E0-0C-7E-54-36-6A-69-F6-3F-13-EF-00-00-00-00-49-45-4E-44-AE-42-60-82",
                ["Star"] = "89-50-4E-47-0D-0A-1A-0A-00-00-00-0D-49-48-44-52-00-00-00-20-00-00-00-20-08-06-00-00-00-73-7A-7A-F4-00-00-00-09-70-48-59-73-00-00-0B-13-00-00-0B-13-01-00-9A-9C-18-00-00-00-01-73-52-47-42-00-AE-CE-1C-E9-00-00-00-04-67-41-4D-41-00-00-B1-8F-0B-FC-61-05-00-00-01-16-49-44-41-54-78-01-ED-94-6D-0D-C2-30-10-86-DF-11-04-20-61-12-70-C0-1C-50-07-AB-03-90-80-04-50-00-28-19-0E-90-00-0E-C0-C1-71-CD-BA-B0-B1-86-B5-BD-0E-FE-EC-49-2E-6D-2E-D7-CB-F5-BE-80-89-09-01-44-94-1B-81-80-19-64-28-16-0D-01-73-C8-28-59-9E-F8-07-36-FD-0D-39-22-91-94-40-B5-EE-1A-91-48-02-58-B7-EE-2B-FC-92-8F-F4-37-2C-10-41-6C-06-0A-87-4E-23-82-DE-14-F0-4F-0A-3E-F2-81-77-1B-87-AE-E4-B7-43-13-71-CF-B2-EC-F2-D5-C2-A4-92-E5-44-E9-39-05-95-89-8D-B7-2C-0F-92-63-7C-6C-11-03-D5-CD-76-A3-78-AE-24-5C-D5-4D-20-3B-0A-67-8F-94-B0-43-E5-99-0D-63-53-60-0C-D8-71-E5-11-40-85-31-A0-7A-3A-7C-30-4D-E7-DD-ED-21-8B-48-79-DA-2D-02-6C-83-02-58-3B-74-07-96-B3-43-5F-22-25-8E-F4-77-66-9B-EF-9A-BA-3B-23-A8-0C-3E-01-E8-96-73-E7-6C-53-7F-67-68-A4-82-9D-1D-AD-D3-C1-D9-A6-F7-CE-38-22-15-F6-D7-45-80-BD-B2-6F-E4-65-60-27-4B-8A-58-A7-B6-24-E9-FA-60-62-62-2C-5E-30-1D-6B-34-83-5B-F0-2B-00-00-00-00-49-45-4E-44-AE-42-60-82",
                ["Star Hollow"] = "89-50-4E-47-0D-0A-1A-0A-00-00-00-0D-49-48-44-52-00-00-00-20-00-00-00-20-08-06-00-00-00-73-7A-7A-F4-00-00-00-09-70-48-59-73-00-00-0B-13-00-00-0B-13-01-00-9A-9C-18-00-00-00-01-73-52-47-42-00-AE-CE-1C-E9-00-00-00-04-67-41-4D-41-00-00-B1-8F-0B-FC-61-05-00-00-01-5A-49-44-41-54-78-01-ED-96-FD-6D-C2-30-10-C5-2F-15-03-B0-41-33-42-47-48-37-C8-06-64-03-BA-41-D9-80-6E-00-1B-B4-9D-20-DD-20-EA-04-C9-06-65-83-EB-3B-F1-2C-8C-04-F9-B0-AD-F0-4F-7E-D2-29-16-3A-5F-EC-77-1F-41-64-61-21-02-55-CD-CD-24-82-27-89-A3-84-55-12-C1-4A-E2-D8-C0-4E-F2-08-28-BF-23-97-40-62-52-60-F2-77-72-56-A0-92-40-32-09-04-B7-AE-79-00-8B-F1-9C-65-D9-AB-CC-85-27-7F-09-2B-B8-5E-CB-5C-E0-65-15-EC-8F-EB-B5-AD-61-6F-12-C0-EA-46-F0-02-8F-7C-60-DF-16-F6-65-0B-48-7F-C2-9E-6F-2C-37-78-0E-75-44-07-FF-9F-5E-0F-DE-E8-A8-C3-94-FE-A1-47-F8-1F-27-A5-C9-24-A5-B4-6D-48-9B-B1-4E-9A-98-F4-B8-20-ED-D4-20-F0-DD-72-4F-A3-91-A3-DA-05-DC-51-C6-43-5F-40-A6-EF-40-DF-0F-49-09-5B-CE-D4-68-7B-7C-1A-FA-14-32-92-D1-93-10-D5-6B-55-DF-C1-7E-7B-DC-AC-0B-86-2B-3D-04-CA-6B-54-DE-6F-57-9F-63-AF-70-D3-0F-25-3D-0F-1F-75-2F-64-EB-B9-2E-A9-EE-1D-32-E5-01-3E-61-35-5F-B2-77-85-A6-97-99-F1-4E-3F-F3-A9-25-25-DE-CD-76-B7-7A-9B-EA-38-35-F6-C9-D3-E0-C9-AF-F7-7A-DB-9B-19-9A-3C-0D-53-7A-5B-BD-99-21-A9-E0-AD-8B-09-FE-25-F7-C4-A7-01-41-5E-34-FC-5B-30-DF-7F-84-85-85-50-FE-01-12-E7-01-A3-5F-51-F9-4C-00-00-00-00-49-45-4E-44-AE-42-60-82",
            };

        }



        #endregion

        #region Other


        public static void MarkInteractive(this Rect rect)
        {
            if (!curEvent.isRepaint) return;

            var unclippedRect = (Rect)_mi_GUIClip_UnclipToWindow.Invoke(null, new object[] { rect });

            var curGuiView = _pi_GUIView_current.GetValue(null);

            _mi_GUIView_MarkHotRegion.Invoke(curGuiView, new object[] { unclippedRect });

        }

        static PropertyInfo _pi_GUIView_current = typeof(Editor).Assembly.GetType("UnityEditor.GUIView").GetProperty("current", maxBindingFlags);
        static MethodInfo _mi_GUIView_MarkHotRegion = typeof(Editor).Assembly.GetType("UnityEditor.GUIView").GetMethod("MarkHotRegion", maxBindingFlags);
        static MethodInfo _mi_GUIClip_UnclipToWindow = typeof(GUI).Assembly.GetType("UnityEngine.GUIClip").GetMethod("UnclipToWindow", maxBindingFlags, null, new[] { typeof(Rect) }, null);





        #endregion

    }


}
#endif