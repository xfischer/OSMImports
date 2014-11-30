﻿/*
 * Copyright (c) 2014, Achim 'ahzf' Friedland <achim@graphdefined.org>
 * This file is part of OpenDataAPI <http://www.github.com/GraphDefined/OpenDataAPI>
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
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

#endregion

namespace org.GraphDefined.OpenDataAPI.OverpassAPI
{

    /// <summary>
    /// The JSON result of an Overpass query.
    /// </summary>
    public static partial class OverpassAPIExtentions
    {

        #region ToFile(this OverpassQuery, Filename)

        /// <summary>
        /// Run the given Overpass query and write the result to the given file.
        /// </summary>
        /// <param name="OverpassQuery">An Overpass query.</param>
        /// <param name="Filename">A file name.</param>
        public static Task<OverpassResult> ToFile(this OverpassQuery OverpassQuery, String Filename)
        {

            return OverpassQuery.
                       RunQuery().
                       ToFile(Filename);

        }

        #endregion

        #region ToFile(this ResultTask, Filename)

        /// <summary>
        /// Write the given Overpass query result to the given file.
        /// </summary>
        /// <param name="ResultTask">A Overpass query result task.</param>
        /// <param name="Filename">A file name.</param>
        public static Task<OverpassResult> ToFile(this Task<OverpassResult>  ResultTask,
                                                  String                     Filename)
        {

            return ResultTask.ContinueWith(task => {
                                               File.WriteAllText(Filename, ResultTask.Result.ToJSON().ToString());
                                               return ResultTask.Result;
                                           });

        }

        #endregion


        #region ToFile(this JSONTask, Filename)

        /// <summary>
        /// Write the given JSON query result to the given file.
        /// </summary>
        /// <param name="JSONTask">A JSON query result task.</param>
        /// <param name="Filename">A file name.</param>
        public static Task<JObject> ToFile(this Task<JObject> JSONTask, String Filename)
        {

            return JSONTask.ContinueWith(task => {
                                            File.WriteAllText(Filename, JSONTask.Result.ToString());
                                            return JSONTask.Result;
                                        });

        }

        #endregion

    }

}
