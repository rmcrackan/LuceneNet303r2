/*
 *
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 *
*/

using System;
using System.Configuration;

namespace Lucene.Net.Support
{
    /// <summary>
    /// 
    /// </summary>
    public class AppSettings
    {
        static System.Collections.Specialized.ListDictionary settings = new System.Collections.Specialized.ListDictionary();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        public static void Set(string key, int defValue)
        {
            settings[key] = defValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        public static void Set(string key, long defValue)
        {
            settings[key] = defValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        public static void Set(string key, string defValue)
        {
            settings[key] = defValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        public static void Set(string key, bool defValue)
        {
            settings[key] = defValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int Get(string key, int defValue)
        {
            if (settings[key] != null)
            {
                return (int)settings[key];
            }

			string theValue = ConfigurationManager.AppSettings.Get(key);
            if (theValue == null)
            {
                return defValue;
            }
            int retValue = Convert.ToInt32(theValue.Trim());
            settings[key] = retValue;
            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static long Get(string key, long defValue)
        {
            if (settings[key] != null)
            {
                return (long)settings[key];
            }

			string theValue = ConfigurationManager.AppSettings.Get(key);
            if (theValue == null)
            {
                return defValue;
            }
            long retValue = Convert.ToInt64(theValue.Trim());
            settings[key] = retValue;
            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string Get(string key, string defValue)
        {
            if (settings[key] != null)
            {
                return (string)settings[key];
            }

			string theValue = ConfigurationManager.AppSettings.Get(key);
            if (theValue == null)
            {
                return defValue;
            }
            settings[key] = theValue;
            return theValue;
        }

        public static bool Get(string key, bool defValue)
        {
            if (settings[key] != null)
            {
                return (bool)settings[key];
            }

			string theValue = ConfigurationManager.AppSettings.Get(key);
            if (theValue == null)
            {
                return defValue;
            }
            bool retValue = Convert.ToBoolean(theValue.Trim());
            settings[key] = retValue;
            return retValue;
        }
    }
}
