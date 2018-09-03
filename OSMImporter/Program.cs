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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
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
        public static void RunAll(this OverpassQuery  OverpassQuery,
                                  String              FilenamePrefix)
        {

            OverpassQuery.
                ToFile       (FilenamePrefix + ".json").
                ToGeoJSONFile(FilenamePrefix + ".geojson").
                ContinueWith(task => Console.WriteLine(FilenamePrefix + ".* files are ready!")).
                Wait();

        }

        #endregion

    }

    public struct Hebesaetze
    {

        public UInt32 Id;
        public String Name;
        public UInt16 Jahr;
        public UInt16 GrundsteuerA;
        public UInt16 GrundsteuerB;
        public UInt16 Gewerbesteuer;

        public Hebesaetze(UInt32 _Id,
                          String _Name,
                          UInt16 _Jahr,
                          UInt16 _GrundsteuerA,
                          UInt16 _GrundsteuerB,
                          UInt16 _Gewerbesteuer)
        {
            Id             = _Id;
            Name           = _Name;
            Jahr           = _Jahr;
            GrundsteuerA   = _GrundsteuerA;
            GrundsteuerB   = _GrundsteuerB;
            Gewerbesteuer  = _Gewerbesteuer;
        }

        public override String ToString()
        {
            return "'" + Name + "' (" + Id + ") in " + Jahr;
        }

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

            var JenaId      = new OverpassQuery("Jena").AreaId;
            var ThüringenId = new OverpassQuery("Thüringen").AreaId;

            Directory.CreateDirectory("Getränke");
            Directory.CreateDirectory("Gemeinschaftsanlage");
            Directory.CreateDirectory("Freizeit");
            Directory.CreateDirectory("Gebäude");
            Directory.CreateDirectory("Bundestagswahlkreise");
            Directory.CreateDirectory("Gebietsgrenzen");
            Directory.CreateDirectory("Gebietsgrenzen/Ortsteile_Jena");
            Directory.CreateDirectory("ÖffentlicherNahverkehr");
            Directory.CreateDirectory("Strassen");
            Directory.CreateDirectory("Tempolimits");
            Directory.CreateDirectory("Energie");
            Directory.CreateDirectory("Flächennutzung");
            Directory.CreateDirectory("Flächennutzung/Grünfläche");
            Directory.CreateDirectory("Flächennutzung/Wasser");
            Directory.CreateDirectory("Flächennutzung/Gewerbe");
            Directory.CreateDirectory("Flächennutzung/Wohngebiete");
            Directory.CreateDirectory("Flächennutzung/Militär");


			#region Freizeit

			new OverpassQuery(JenaId).
				WithAny("drink:club-mate").
				RunAll("Getränke/club-mate");

            #endregion


            #region Hebesätze der Gemeinden

            //new OverpassQuery(ThüringenId).
            //    // Bei admin_level = 8 fehlen die großen Städte!
            //    WithRelations("boundary",    "administrative").And("admin_level", "8").
            //    WithRelations("boundary",    "administrative").And("name",        "Eisenach").
            //    WithRelations("boundary",    "administrative").And("name",        "Erfurt").
            //    WithRelations("boundary",    "administrative").And("name",        "Weimar").
            //    WithRelations("boundary",    "administrative").And("name",        "Jena").
            //    WithRelations("boundary",    "administrative").And("name",        "Gera").
            //    WithRelations("boundary",    "administrative").And("name",        "Suhl").
            //    RunAll       ("Gebietsgrenzen/Thüringen_Gemeinden",
            //                  SecretKey, Passphrase);

            // http://www.statistik.thueringen.de/datenbank/TabAnzeige.asp?tabelle=GE001613%7C%7CHebes%E4tze+der+Gemeinden&startpage=99&csv=&richtung=&sortiere=&vorspalte=0&tit2=&TIS=&SZDT=&anzahlH=-1&fontgr=12&mkro=&AnzeigeAuswahl=&XLS=&auswahlNr=&felder=0&felder=1&felder=2&zeit=2013%7C%7C99

            var Hebesaetze_Jahr_Id  = new Dictionary<UInt16, Dictionary<UInt32, Hebesaetze>>();
            var Hebesaetze_Id_Jahr  = new Dictionary<UInt32, Dictionary<UInt16, Hebesaetze>>();

            Directory.EnumerateFiles("Hebesätze der Gemeinden", "*.csv").ForEach(InFile => {

                //OpenGPG.CreateSignature(File.OpenRead (InFile),
                //                        File.OpenWrite(InFile.Replace(".csv", ".sig")),
                //                        SecretKey,
                //                        Passphrase,
                //                        HashAlgorithms.Sha512,
                //                        ArmoredOutput: true);

                //OpenGPG.CreateSignature(File.OpenRead (InFile),
                //                        File.OpenWrite(InFile.Replace(".csv", ".bsig")),
                //                        SecretKey,
                //                        Passphrase,
                //                        HashAlgorithms.Sha512,
                //                        ArmoredOutput: false);

                var Splitter  = new Char[1] { ';' };
                var Splitted  = new String[0];

                var Jahr      = UInt16.Parse(InFile.Replace("Hebesätze der Gemeinden\\Hebesätze der Gemeinden_", "").
                                                    Replace(".csv", ""));

                UInt32                           Id;
                Hebesaetze                         _Hebesaetze;
                Dictionary<UInt32, Hebesaetze>   Id_Hebesaetze;
                Dictionary<UInt16, Hebesaetze> Jahr_Hebesaetze;

                foreach (var Line in File.ReadLines(InFile))
                {

                    Splitted  = Line.Split(Splitter, StringSplitOptions.None);
                    Id        = UInt32.Parse(Splitted[0]);

                    if (!Hebesaetze_Jahr_Id.TryGetValue(Jahr, out Id_Hebesaetze))
                        Id_Hebesaetze   = Hebesaetze_Jahr_Id.AddAndReturnValue(Jahr, new Dictionary<UInt32, Hebesaetze>());

                    if (!Hebesaetze_Id_Jahr.TryGetValue(Id,   out Jahr_Hebesaetze))
                        Jahr_Hebesaetze = Hebesaetze_Id_Jahr.AddAndReturnValue(Id,   new Dictionary<UInt16, Hebesaetze>());

                    UInt16 GrundsteuerA;
                    UInt16 GrundsteuerB;
                    UInt16 Gewerbesteuer;

                    UInt16.TryParse(Splitted[2], out GrundsteuerA);
                    UInt16.TryParse(Splitted[3], out GrundsteuerB);
                    UInt16.TryParse(Splitted[4], out Gewerbesteuer);

                    _Hebesaetze = new Hebesaetze(Id,
                                                 Splitted[1],
                                                 Jahr,
                                                 GrundsteuerA,
                                                 GrundsteuerB,
                                                 Gewerbesteuer);

                      Id_Hebesaetze.Add(Id,   _Hebesaetze);
                    Jahr_Hebesaetze.Add(Jahr, _Hebesaetze);

                }

            });

            var Gemeinden_JSON = JObject.Parse(File.ReadAllText("Gebietsgrenzen/Thüringen_Gemeinden.geojson"));
            Dictionary<UInt16, Hebesaetze> HebesätzeNachJahr = null;
            UInt16 ExportJahr = 20130;

            foreach (var GeoJSONFeature in Gemeinden_JSON["features"].Children<JObject>())
            {

                // 16 == Thüringen
                //  0 == Regierungsbezirke (gibt's aber in Thüringen nicht)
                var AmtlicherGemeindeschluessel = UInt32.Parse(GeoJSONFeature["properties"]["de:amtlicher_gemeindeschluessel"].Value<String>().Replace("160", ""));

                if (Hebesaetze_Id_Jahr.TryGetValue(AmtlicherGemeindeschluessel, out HebesätzeNachJahr))
                {

                    var GeoJSON_Properties = GeoJSONFeature["properties"] as JObject;

                    GeoJSON_Properties.
                        Add("Hebesätze", new JObject(HebesätzeNachJahr.
                                                         Values.
                                                         Select(v => new JProperty(v.Jahr.ToString(),
                                                             new JObject(new JProperty("GrundsteuerA", v.GrundsteuerA),
                                                                         new JProperty("GrundsteuerB",  v.GrundsteuerB),
                                                                         new JProperty("Gewerbesteuer", v.Gewerbesteuer))))));

                    // Nur für CartoDB!
                    //if (HebesätzeNachJahr.ContainsKey(ExportJahr)) {
                    //    GeoJSON_Properties.Add("GrundsteuerA_"  + ExportJahr, HebesätzeNachJahr[ExportJahr].GrundsteuerA);
                    //    GeoJSON_Properties.Add("GrundsteuerB_"  + ExportJahr, HebesätzeNachJahr[ExportJahr].GrundsteuerB);
                    //    GeoJSON_Properties.Add("Gewerbesteuer_" + ExportJahr, HebesätzeNachJahr[ExportJahr].Gewerbesteuer);
                    //}

                }

            }

            File.WriteAllText("Gebietsgrenzen/Thüringen_Gemeinden_mitHebesätzen_" + ExportJahr + ".geojson", Gemeinden_JSON.ToString());

			#endregion


			#region Gemeinschaftsanlage

			// cat amenity.json|grep amenity|sed -e 's/,//g'|sort|uniq
			//new OverpassQuery(JenaId).
			//    WithAny      ("amenity").
			//    ToFile       ("Gemeinschaftsanlage/amenity.json").
			//    RunNow();

			new OverpassQuery(JenaId).
				WithAny("amenity", "Cocktailbar").
				RunAll("Gemeinschaftsanlage/amenity.cocktailbar");

			new OverpassQuery(JenaId).
				WithAny("amenity", "animal_shelter").
				RunAll("Gemeinschaftsanlage/amenity.animal_shelter");

          

            #endregion
			

            #region Gebietsgrenzen

          

            new OverpassQuery(ThüringenId).
                // Bei admin_level = 8 fehlen die großen Städte!
                WithRelations("boundary",    "administrative").And("admin_level", "8").
                WithRelations("boundary",    "administrative").And("name",        "Eisenach").
                WithRelations("boundary",    "administrative").And("name",        "Erfurt").
                WithRelations("boundary",    "administrative").And("name",        "Weimar").
                WithRelations("boundary",    "administrative").And("name",        "Jena").
                WithRelations("boundary",    "administrative").And("name",        "Gera").
                WithRelations("boundary",    "administrative").And("name",        "Suhl").
                RunAll       ("Gebietsgrenzen/Thüringen_Gemeinden");


            // Jena

            new OverpassQuery(JenaId).
                WithRelations("boundary",    "administrative").
                And          ("admin_level", "9").
                RunAll       ("Gebietsgrenzen/Ortsteile_Jena/Jena_Ortsteile");

            // ToDo: Missing OpenGPG signing for splitted GeoJSON has to be implemented!
            new OverpassQuery(JenaId).
                WithRelations("boundary",    "administrative").
                And          ("admin_level", "9").
                ToGeoJSON    ().
                SplitFeatures().
                ToGeoJSONFile(JSON => "Gebietsgrenzen/Ortsteile_Jena/" + JSON["features"][0]["properties"]["name"].ToString().Replace("/", "_").Replace(" ", "") + ".geojson").
                RunNow();

            #endregion
			
            // -----------------------------------------------------------------

            Console.WriteLine("ready...");
            Console.ReadLine();

        }

    }

}
