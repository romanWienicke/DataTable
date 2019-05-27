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
    public static class QueryCache
    {
        public static async Task<List<T>> FromCache<T>(this IQueryable<T> source, string cacheFileName, bool clear = false) where T : class
        {
            if (!File.Exists(cacheFileName) || clear)
            {
                var data = await source.ToListAsync();
               
                using (var sw = new StreamWriter(cacheFileName, !clear))
                {

                    await sw.WriteAsync(JsonConvert.SerializeObject(data, new JsonSerializerSettings {
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    }));
                }
                return data;
            }

            using (var sr = new StreamReader(cacheFileName))
            {
                return JsonConvert.DeserializeObject<List<T>>(
                    await sr.ReadToEndAsync()
                    );
            }
        }

        public static async Task<T> FromCacheSingle<T>(this T data, string cacheFileName, bool clear = false) where T : class
        {

            if (!File.Exists(cacheFileName) || clear)
            {

                using (var sw = new StreamWriter(cacheFileName, !clear))
                {

                    await sw.WriteAsync(JsonConvert.SerializeObject(data, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    }));
                }
                return data;
            }

            using (var sr = new StreamReader(cacheFileName))
            {
                return JsonConvert.DeserializeObject<T>(
                    await sr.ReadToEndAsync()
                    );
            }
        }
    }
}
