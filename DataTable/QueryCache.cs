using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using System.ComponentModel;

namespace Connexion.DataExtensions
{
    /// <summary>
    /// Extenisions for caching Entity Framwork Objects to JSON files
    /// Primary for database depending testing / not for production!
    /// </summary>
    public static class JsonQueryCache
    {
        /// <summary>
        /// Extension for getting a List of<typeparamref name="T"/> from database or JSON
        /// </summary>
        /// <typeparam name="T">Type of T</typeparam>
        /// <param name="source">the IQueryable Source</param>
        /// <param name="cacheFileName">the JSON filename</param>
        /// <param name="clear">if yes clear the cache and create new one</param>
        /// <returns></returns>
        public static async Task<List<T>> FromJson<T>(this IQueryable<T> source, string cacheFileName, bool clear = false) where T : class
        {
            
            // test if cachefile exists or if clear is set
            if (!File.Exists(cacheFileName) || clear)
            {
                // get data from database
                // to list to get all items
                var data = await source.ToListAsync();
                await SaveToJsonFile(cacheFileName, clear, data);

                return data;
            }

            // if the file is present, deserialize from file and return 
            using (var sr = new StreamReader(cacheFileName))
            {
                return JsonConvert.DeserializeObject<List<T>>(
                    await sr.ReadToEndAsync()
                    );
            }
        }

        /// <summary>
        /// Extension for getting as single item of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">type parameter</typeparam>
        /// <param name="data">the single object</param>
        /// <param name="cacheFileName">the JSON filename</param>
        /// <param name="clear">if yes clear the cache and create new one</param>
        /// <returns></returns>
        public static async Task<T> FromJsonSingle<T>(this T data, string cacheFileName, bool clear = false) where T : class
        {
            // test if cachefile exists or if clear is set
            if (!File.Exists(cacheFileName) || clear)
            {
                await SaveToJsonFile(cacheFileName, clear, data);
                return data;
            }

            using (var sr = new StreamReader(cacheFileName))
            {
                return JsonConvert.DeserializeObject<T>(
                    await sr.ReadToEndAsync()
                    );
            }
        }

        /// <summary>
        /// save object to JSON File
        /// </summary>
        /// <typeparam name="T">type parameter</typeparam>
        /// <param name="data">the single object</param>
        /// <param name="cacheFileName">the JSON filename</param>
        /// <param name="clear">if yes clear the cache and create new one</param>
        /// <returns></returns>
        private static async Task SaveToJsonFile<T>(string cacheFileName, bool clear, T data) where T : class
        {
            // create writer for serialization to JSON
            using (var sw = new StreamWriter(cacheFileName, !clear))
            {
                /*
                 serialize the object list
                 Json Settings ReferenceLoopHandling and PreserveReferencesHandling set for allowing the JSON to store object references and not to run into reference loop errors.
                 */
                await sw.WriteAsync(JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                }));
            }
        }

    }
}
