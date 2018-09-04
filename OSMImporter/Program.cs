/*
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
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.OpenDataAPI.OverpassAPI;

#endregion

namespace org.GraphDefined.OpenDataAPI.OSMImporter
{

	public static class Extentions
	{

		#region RunAll(this OverpassQuery, Filename)

		/// <summary>
		/// Standard workflow...
		/// </summary>
		/// <param name="OverpassQuery">An Overpass query.</param>
		/// <param name="FilenamePrefix">A file name prefix.</param>
		public static void RunAll(this OverpassQuery OverpassQuery,
								  String FilenamePrefix)
		{

			OverpassQuery.
				ToFile(FilenamePrefix + ".json").
				ToGeoJSONFile(FilenamePrefix + ".geojson").
				ContinueWith(task => Console.WriteLine(FilenamePrefix + ".* files are ready!")).
				Wait();

		}

		#endregion

	}


	/// <summary>
	/// A little demo... can be tested via http://overpass-turbo.eu
	/// </summary>
	public class Program
	{

		/// <summary>
		/// Main...
		/// </summary>
		/// <param name="Arguments">CLI arguments...</param>
		public static void Main(String[] Arguments)
		{


			Directory.CreateDirectory("output");
			BoundingBox bboxLauzannier = new BoundingBox(44.34815879690078, 6.780796051025391, 44.45878010882453, 6.961898803710937);
			 new OverpassQuery(bboxLauzannier).WithNodes("natural", "peak")
				.RunAll("output/natural.peak");

			new OverpassQuery(bboxLauzannier).WithNodes("natural", "saddle")
			   .RunAll("output/natural.saddle");

			new OverpassQuery(bboxLauzannier).WithWays("waterway", "river")
			   .RunAll("output/waterway.river");

			new OverpassQuery(bboxLauzannier).WithWays("natural", "water")
			   .RunAll("output/natural.water");

			new OverpassQuery(bboxLauzannier).WithRelations("landuse", "reservoir")
			   .RunAll("output/landuse.reservoir");

			new OverpassQuery(bboxLauzannier)
				.WithNodes("natural", "peak")
				.WithNodes("natural", "saddle")
				.WithWays("waterway", "river")
				.WithWays("natural", "water")
				.WithRelations("landuse", "reservoir")
				.WithRelations("natural", "water")
			   .RunAll("output/full");



			// -----------------------------------------------------------------

			Console.WriteLine("ready...");
			Console.ReadLine();

		}

	}

}
