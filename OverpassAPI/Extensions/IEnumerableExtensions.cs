﻿/*
 * Copyright (c) 2010-2014 Achim 'ahzf' Friedland <achim@graphdefined.org>
 * This file is part of Illias <http://www.github.com/Vanaheimr/Illias>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

#endregion

namespace org.GraphDefined.Vanaheimr.Illias
{

    /// <summary>
    /// Extensions to the IEnumerable interface.
    /// </summary>
    public static class IEnumerableExtensions
    {

        #region StdDev(this IEnumerable)

        /// <summary>
        /// Calculates the standard deviation of the given enumeration of doubles.
        /// </summary>
        /// <param name="IEnumerable">An enumeration of Doubles.</param>
        /// <returns>The stddev of the given enumeration of doubles.</returns>
        public static Double StdDev(this IEnumerable<Double> IEnumerable)
        {
            return IEnumerable.AverageAndStdDev().Item2;
        }

        #endregion

        #region AverageAndStdDev(this IEnumerable)

        /// <summary>
        /// Calculates the standard deviation of the given enumeration of doubles.
        /// </summary>
        /// <param name="IEnumerable">An enumeration of Doubles.</param>
        /// <returns>The mean and stddev of the given enumeration of doubles.</returns>
        public static Tuple<Double, Double> AverageAndStdDev(this IEnumerable<Double> IEnumerable)
        {

            #region Initial Checks

            if (IEnumerable == null)
                throw new ArgumentNullException("The given enumeration of doubles must not be null!");

            var Count = IEnumerable.Count();

            if (Count == 0)
                throw new ArgumentNullException("The length of the given enumeration of doubles must not be zero!");

            if (Count == 1)
                return new Tuple<Double, Double>(IEnumerable.First(), 0);

            #endregion

            var average = IEnumerable.Average();
            var sum = 0.0;

            foreach (var value in IEnumerable)
                sum += (value - average) * (value - average);

            return new Tuple<Double, Double>(average, Math.Sqrt(sum / (Count - 1)));

        }

        #endregion


        #region ForEach<T>(this IEnumerable, Action)

        /// <summary>
        /// Calls the given delegate for each element of the enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration of type T.</param>
        /// <param name="Action">An action to call for each element of the enumeration.</param>
        public static void ForEach<T>(this IEnumerable<T> IEnumerable, Action<T> Action)
        {

            #region Initial checks

            if (IEnumerable == null)
                throw new ArgumentNullException("The given IEnumerable must not be null!");

            if (Action == null)
                throw new ArgumentNullException("The given Action must not be null!");

            #endregion

            foreach (var Element in IEnumerable)
                Action(Element);

        }

        #endregion

        #region TryForEach<T>(this IEnumerable, Action)

        /// <summary>
        /// Calls the given delegate for each element of the enumeration,
        /// but does not fail if any parameter is null.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration of type T.</param>
        /// <param name="Action">An action to call for each element of the enumeration.</param>
        public static void TryForEach<T>(this IEnumerable<T> IEnumerable, Action<T> Action)
        {
            if (IEnumerable != null && Action != null)
                foreach (var Element in IEnumerable)
                    Action(Element);
        }

        #endregion

        #region ForEach<S, T>(this IEnumerable, Seed, Action)

        /// <summary>
        /// Calls the given delegate for each element of the enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration of type T.</param>
        /// <param name="Action">An action to call for each element of the enumeration.</param>
        public static S ForEach<S, T>(this IEnumerable<T> IEnumerable, S Seed, Action<S, T> Action)
        {

            #region Initial checks

            if (IEnumerable == null)
                throw new ArgumentNullException("The given IEnumerable must not be null!");

            if (Action == null)
                throw new ArgumentNullException("The given Action must not be null!");

            #endregion

            S _Seed = Seed;

            foreach (var Item in IEnumerable)
                Action(_Seed, Item);

            return _Seed;

        }

        #endregion

        #region ForEachCounted<T>(this IEnumerable, Action, Counter = 1UL)

        /// <summary>
        /// Calls the given delegate for each element of the enumeration
        /// and count the number of elements.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration of type T.</param>
        /// <param name="Action">An action to call for a counter and each element of the enumeration.</param>
        /// <param name="Counter">The initial value of the counter.</param>
        public static void ForEachCounted<T>(this IEnumerable<T> IEnumerable, Action<UInt64, T> Action, UInt64 Counter = 1UL)
        {

            if (IEnumerable == null || Action == null)
                return;

            foreach (var Element in IEnumerable)
                Action(Counter++, Element);

        }

        #endregion

        #region ForEach<T>(this IEnumerable, First, Remaining)

        /// <summary>
        /// Calls the given delegate for each element of the enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration of type T.</param>
        /// <param name="First">An action to call for the first element of the enumeration.</param>
        /// <param name="Remaining">An action to call for each element except the first element of the enumeration.</param>
        public static void ForEach<T>(this IEnumerable<T> IEnumerable, Action<T> First, Action<T> Remaining)
        {

            #region Initial checks

            if (IEnumerable == null)
                throw new ArgumentNullException("The given IEnumerable must not be null!");

            if (First == null)
                throw new ArgumentNullException("The given Action must not be null!");

            if (Remaining == null)
                throw new ArgumentNullException("The given Action must not be null!");

            #endregion

            if (!IEnumerable.Any())
                return;

            First(IEnumerable.First());

            foreach (var Element in IEnumerable.Skip(1))
                Remaining(Element);

        }

        #endregion

        #region SelectCounted<T1, T2>(this IEnumerable, Delegate, Counter = 1UL)

        /// <summary>
        /// Calls the given delegate for each element of the enumeration
        /// and count the number of elements.
        /// </summary>
        /// <typeparam name="T1">The type of the enumeration.</typeparam>
        /// <typeparam name="T2">The type of the result enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration of type T.</param>
        /// <param name="Delegate">A delegate to call for a counter and each element of the enumeration.</param>
        /// <param name="Counter">The initial value of the counter.</param>
        public static IEnumerable<T2> SelectCounted<T1, T2>(this IEnumerable<T1> IEnumerable, Func<UInt64, T1, T2> Delegate, UInt64 Counter = 1UL)
        {

            if (IEnumerable == null || Delegate == null)
                yield break;

            if (IEnumerable.Any())
                foreach (var Element in IEnumerable)
                    yield return Delegate(Counter++, Element);

        }

        #endregion


        #region Skip<T>(this IEnumerable, Count)

        /// <summary>
        /// Skips the given number of elements in the enumeration.
        /// </summary>
        /// <typeparam name="T">The type fo the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration.</param>
        /// <param name="Count">The number of elements to skip.</param>
        public static IEnumerable<T> Skip<T>(this IEnumerable<T> IEnumerable, UInt32 Count)
        {

            var IEnumerator = IEnumerable.GetEnumerator();

            for (var i = 0U; i<Count; i++)
                IEnumerator.MoveNext();

            while (IEnumerator.MoveNext())
                yield return IEnumerator.Current;

        }

        /// <summary>
        /// Skips the given number of elements in the enumeration.
        /// </summary>
        /// <typeparam name="T">The type fo the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration.</param>
        /// <param name="Count">The number of elements to skip.</param>
        public static IEnumerable<T> Skip<T>(this IEnumerable<T> IEnumerable, UInt64 Count)
        {

            var IEnumerator = IEnumerable.GetEnumerator();

            for (var i = 0UL; i < Count; i++)
                IEnumerator.MoveNext();

            while (IEnumerator.MoveNext())
                yield return IEnumerator.Current;

        }

        /// <summary>
        /// Skips the given number of elements in the enumeration.
        /// </summary>
        /// <typeparam name="T">The type fo the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration.</param>
        /// <param name="Count">The number of elements to skip.</param>
        public static IEnumerable<T> Skip<T>(this IEnumerable<T> IEnumerable, Int64 Count)
        {

            var IEnumerator = IEnumerable.GetEnumerator();

            for (var i = 0L; i < Count; i++)
                IEnumerator.MoveNext();

            while (IEnumerator.MoveNext())
                yield return IEnumerator.Current;

        }

        #endregion

        #region Take<T>(this IEnumerable, Count)

        /// <summary>
        /// Takes the given number of elements from the enumeration.
        /// </summary>
        /// <typeparam name="T">The type fo the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration.</param>
        /// <param name="Count">The number of elements to skip.</param>
        public static IEnumerable<T> Take<T>(this IEnumerable<T> IEnumerable, UInt32 Count)
        {

            var IEnumerator = IEnumerable.GetEnumerator();

            for (var i = 0U; i < Count; i++)
            {

                if (IEnumerator.MoveNext())
                    yield return IEnumerator.Current;

                else
                    break;

            }

        }

        /// <summary>
        /// Skips the given number of elements in the enumeration.
        /// </summary>
        /// <typeparam name="T">The type fo the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration.</param>
        /// <param name="Count">The number of elements to skip.</param>
        public static IEnumerable<T> Take<T>(this IEnumerable<T> IEnumerable, UInt64 Count)
        {

            var IEnumerator = IEnumerable.GetEnumerator();

            for (var i = 0UL; i < Count; i++)
            {

                if (IEnumerator.MoveNext())
                    yield return IEnumerator.Current;

                else
                    break;

            }

        }

        /// <summary>
        /// Skips the given number of elements in the enumeration.
        /// </summary>
        /// <typeparam name="T">The type fo the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration.</param>
        /// <param name="Count">The number of elements to skip.</param>
        public static IEnumerable<T> Take<T>(this IEnumerable<T> IEnumerable, Int64 Count)
        {

            var IEnumerator = IEnumerable.GetEnumerator();

            for (var i = 0L; i < Count; i++)
            {

                if (IEnumerator.MoveNext())
                    yield return IEnumerator.Current;

                else
                    break;

            }

        }

        #endregion

        #region IsNullOrEmpty<T>(this myEnumerable)

        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> myEnumerable)
        {

            if (myEnumerable == null || !myEnumerable.Any())
                return true;

            return false;

        }

        #endregion

        #region IsNeitherNullNorEmpty<T>(this myEnumerable)

        public static Boolean IsNeitherNullNorEmpty<T>(this IEnumerable<T> myEnumerable)
        {

            if (myEnumerable == null || !myEnumerable.Any())
                return false;

            return true;

        }

        #endregion


        #region CountIsAtLeast<T>(this myIEnumerable, myNumberOfElements)

        public static Boolean CountIsAtLeast<T>(this IEnumerable<T> myIEnumerable, UInt64 myNumberOfElements)
        {

            if (myIEnumerable == null)
                return false;

            var _Enumerator = myIEnumerable.GetEnumerator();

            while (myNumberOfElements > 0 && _Enumerator.MoveNext())
                myNumberOfElements--;

            return (myNumberOfElements == 0 && !_Enumerator.MoveNext());

        }

        #endregion

        #region CountIsGreater<T>(this myIEnumerable, myNumberOfElements)

        public static Boolean CountIsGreater<T>(this IEnumerable<T> myIEnumerable, UInt64 myNumberOfElements)
        {

            if (myIEnumerable == null)
                return false;

            var _Enumerator = myIEnumerable.GetEnumerator();

            while (myNumberOfElements > 0 && _Enumerator.MoveNext())
                myNumberOfElements--;

            return (myNumberOfElements == 0 && _Enumerator.MoveNext());

        }

        #endregion

        #region CountIsGreaterOrEquals<T>(this myIEnumerable, myNumberOfElements)

        public static Boolean CountIsGreaterOrEquals<T>(this IEnumerable<T> myIEnumerable, UInt64 myNumberOfElements)
        {

            if (myIEnumerable == null)
                return false;

            var _Enumerator = myIEnumerable.GetEnumerator();

            while (myNumberOfElements > 0 && _Enumerator.MoveNext())
                myNumberOfElements--;

            return (myNumberOfElements == 0);

        }

        #endregion


        #region When(this Object)

        /// <summary>
        /// Return the given object, when the condition delegate returns true.
        /// Otherwise return default(T).
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="Object">An object.</param>
        /// <param name="ConditionDelegate">A delegate for checking some condition.</param>
        /// <returns>The object if the condition is true; default(T) otherwise.</returns>
        public static T When<T>(this T Object, Func<T, Boolean> ConditionDelegate)
        {

            if (ConditionDelegate == null)
                throw new ArgumentNullException("ConditionDelegate", "The ConditionDelegate must not be null!");

            if (Object == null)
                return default(T);

            if (ConditionDelegate(Object))
                return Object;

            return default(T);

        }

        #endregion

        #region SafeSelect(this IEnumerable, SelectionDelegate)

        /// <summary>
        /// Safely selects the given enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <typeparam name="TResult">The type of the resulting enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration.</param>
        /// <param name="SelectionDelegate">The delegate to select the given enumeration.</param>
        public static IEnumerable<TResult> SafeSelect<T, TResult>(this IEnumerable<T>  IEnumerable,
                                                                  Func<T, TResult>     SelectionDelegate)
        {

            if (IEnumerable == null)
                return null;

            return IEnumerable.Select(SelectionDelegate);

        }

        #endregion

        #region SafeSelect(this IEnumerable, SelectionDelegate)

        /// <summary>
        /// Safely selects the given enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <typeparam name="TResult">The type of the resulting enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration.</param>
        /// <param name="MapDelegate">The delegate to select the given enumeration.</param>
        public static String MapReduce<T>(this IEnumerable<T>  IEnumerable,
                                          Func<T, String>      MapDelegate,
                                          String               Seperator = ", ",
                                          String               Default   = "")
        {

            if (IEnumerable == null)
                return null;

            return IEnumerable.Select(MapDelegate).AggregateOrDefault((a, b) => a + Seperator + b, Default);

        }

        #endregion


        #region Aggregate(this IEnumerable<String>)

        public static String Aggregate(this IEnumerable<String> IEnumerable)
        {

            if (IEnumerable == null || !IEnumerable.Any())
                return String.Empty;

            return String.Concat(IEnumerable.ToArray());

        }

        #endregion

        #region AggregateOrDefault(this IEnumerable, AggreationDelegate, DefaultT = default(T))

        /// <summary>
        /// Safely aggregates the given enumeration. If the enumeration is null
        /// or has no elements the default value will be returned.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration.</param>
        /// <param name="AggreationDelegate">The delegate to aggregate the given enumeration.</param>
        /// <param name="DefaultT">The default value to return for an empty enumeration.</param>
        public static T AggregateOrDefault<T>(this IEnumerable<T>  IEnumerable,
                                              Func<T, T, T>        AggreationDelegate,
                                              T                    DefaultT = default(T))
        {

            if (IEnumerable == null)
                return DefaultT;

            //if (!IEnumerable.Any())
            //    return DefaultT;
            try
            {
                return IEnumerable.Aggregate(AggreationDelegate);
            }
            catch (Exception)
            {
                return DefaultT;
            }

        }

        #endregion

        #region AggregateOrDefault(this IEnumerable, AggreationDelegate, DefaultT = default(T))

        /// <summary>
        /// Safely aggregates the given enumeration. If the enumeration is null
        /// or has no elements the default value will be returned.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration.</param>
        /// <param name="AggreationDelegate">The delegate to aggregate the given enumeration.</param>
        /// <param name="DefaultT">The default value to return for an empty enumeration.</param>
        public static T AggregateOrDefault<T>(this IEnumerable<T>  IEnumerable,
                                              T                    Prefix,
                                              Func<T, T>           Map,
                                              Func<T, T, T>        Reduce,
                                              T                    Suffix,
                                              T                    DefaultValue = default(T))
        {

            if (IEnumerable == null)
                return DefaultValue;

            var Array = IEnumerable.Select(i => Map(i)).ToArray();

            //if (!IEnumerable.Any())
            //    return DefaultT;
            try
            {
                return Reduce(Reduce(Prefix, Array.Aggregate(Reduce)), Suffix);
            }
            catch (Exception)
            {
                return DefaultValue;
            }

        }

        #endregion

        #region AggregateWith(this IEnumerable, Seperator)

        /// <summary>
        /// Safely aggregates the given enumeration. If the enumeration is null
        /// or has no elements an empty string will be returned.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="IEnumerable">An enumeration.</param>
        /// <param name="Seperator">A string as element seperator.</param>
        public static String AggregateWith<T>(this IEnumerable<T>  IEnumerable,
                                              String               Seperator)
        {

            if (IEnumerable == null || !IEnumerable.Any())
                return String.Empty;

            return IEnumerable.
                       Select(v => v.ToString()).
                       Aggregate((a, b) => a + Seperator + b);

        }

        #endregion

        #region CSVAggregate(this IEnumerable<String>)

        public static String CSVAggregate(this IEnumerable<String> IEnumerable)
        {

            if (IEnumerable == null || !IEnumerable.Any())
                return String.Empty;

            return IEnumerable.Aggregate((a, b) => a + ", " + b);

        }

        #endregion

        #region CSVAggregate(this IEnumerable<String>, Prefix, Suffix)

        public static String CSVAggregate(this IEnumerable<String> IEnumerable, String Prefix, String Suffix)
        {

            if (IEnumerable == null || !IEnumerable.Any())
                return Prefix + Suffix;

            return String.Concat(Prefix, IEnumerable.Aggregate((a, b) => a + ", " + b), Suffix);

        }

        #endregion


        #region ToPartitions(this IEnumerable, SizeOfPartition)

        public static IEnumerable<IEnumerable<T>> ToPartitions<T>(this IEnumerable<T> IEnumerable, UInt64 SizeOfPartition)
        {

            UInt64 i;
            T[] Partitions;
            var IEnumerator = IEnumerable.GetEnumerator();

            while (IEnumerator.MoveNext())
            {

                Partitions    = new T[SizeOfPartition];
                Partitions[0] = IEnumerator.Current;
                i             = 1UL;

                while (i < SizeOfPartition && IEnumerator.MoveNext())
                {
                    Partitions[i] = IEnumerator.Current;
                    i = i + 1;
                }

                if (i < SizeOfPartition)
                    Partitions = Partitions.Take(i).ToArray();

                yield return Partitions;

            }

        }

        #endregion

        #region ConsumeAll<T>(this IEnumerator)

        /// <summary>
        /// Consume all elements of the given enumerator.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="Enumerator">An IEnumerator.</param>
        /// <returns>An enumerable of T.</returns>
        public static IEnumerable<T> ConsumeAll<T>(this IEnumerator Enumerator)
        {

            var List = new List<T>();

            while (Enumerator.MoveNext())
                List.Add((T)Enumerator.Current);

            return List;

        }

        #endregion

        #region Swap<T>(this IEnumerator)

        public static IEnumerable<T> Swap<T>(this IEnumerable<T> IEnumerable)
        {

            var Enumerator = IEnumerable.GetEnumerator();

            T a = default(T);
            T b = default(T);
            Byte Emit = 0;

            while (Enumerator.MoveNext())
            {

                if (Emit == 0)
                {
                    a = Enumerator.Current;
                    Emit++;
                }

                else
                {
                    b = Enumerator.Current;
                    yield return b;
                    yield return a;
                    Emit = 0;
                }

            }

            if (Emit == 1)
                yield return a;

        }

        #endregion

        //#region ToHashSet<T>(this Enumeration)

        //public static HashedSet<T> ToHashSet<T>(this IEnumerable<T> Enumeration)
        //{
        //    return new HashedSet<T>(Enumeration);
        //}

        //#endregion


    }

}
