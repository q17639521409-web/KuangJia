using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace FrameWebAPI.Service.Helper
{

    public class JsonHelper
    {
        public static string ObjectToJson(object Obj)
        {
            try
            {
                return JsonConvert.SerializeObject(Obj);
            }
            catch
            {
                return "\"\"";
            }
        }

        public static object JsonToObject(string Json, Type Type)
        {
            try
            {
                return JsonConvert.DeserializeObject(Json, Type);
            }
            catch
            {
                return new object();
            }
        }

        public static TEntity JsonToObject<TEntity>(string Json)
        {
            try
            {
                return JsonConvert.DeserializeObject<TEntity>(Json);
            }
            catch
            {
                return default(TEntity);
            }
        }

        public static byte[] Serialize(object item)
        {
            string s = JsonConvert.SerializeObject(item);
            return Encoding.UTF8.GetBytes(s);
        }

        public static TEntity Deserialize<TEntity>(byte[] value)
        {
            if (value == null)
            {
                return default(TEntity);
            }
            return JsonConvert.DeserializeObject<TEntity>(Encoding.UTF8.GetString(value));
        }

        public static string DataTableToJson(DataTable DataTable)
        {
            try
            {
                return JsonConvert.SerializeObject(new DataSet("DataSet")
                {
                    Tables = { DataTable.Copy() }
                });
            }
            catch (Exception)
            {
                return "\"\"";
            }
        }

        public static DataSet JsonToDataSet(string JsonString)
        {
            try
            {
                new DataSet();
                return (DataSet)JsonConvert.DeserializeObject(JsonString, typeof(DataSet));
            }
            catch
            {
                return new DataSet();
            }
        }

        public static DataTable JsonToDataTable(string JsonString)
        {
            try
            {
                DataSet dataSet = new DataSet();
                dataSet = JsonToDataSet(JsonString);
                if (dataSet.Tables.Count == 1)
                {
                    return dataSet.Tables[0];
                }
                return new DataTable();
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        public static DataTable NewJsonToDataTable(string strJson)
        {
            DataTable dataTable = new DataTable();
            try
            {
                JArray jArray = JsonConvert.DeserializeObject(strJson) as JArray;
                if (jArray.Count > 0)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (JProperty item in (jArray[0] as JObject).AsJEnumerable())
                    {
                        string name = item.Name;
                        stringBuilder.Append(name + Convert.ToString(","));
                        dataTable.Columns.Add(name);
                    }
                    for (int i = 0; i <= jArray.Count - 1; i++)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        foreach (JProperty item2 in (jArray[i] as JObject).AsJEnumerable())
                        {
                            string name2 = item2.Name;
                            if (!dataTable.Columns.Contains(name2))
                            {
                                dataTable.Columns.Add(name2);
                            }
                            string value = item2.Value.ToString();
                            dataRow[name2] = value;
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }
            }
            catch
            {
            }
            return dataTable;
        }

        public static string GetJSON<T>(object obj)
        {
            string empty = string.Empty;
            try
            {
                DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
                using MemoryStream memoryStream = new MemoryStream();
                dataContractJsonSerializer.WriteObject((Stream)memoryStream, obj);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string JSON<T>(List<T> vals)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
                foreach (T val in vals)
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    dataContractJsonSerializer.WriteObject((Stream)memoryStream, (object?)val);
                    stringBuilder.Append(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return stringBuilder.ToString();
        }

        public static T ParseFormByJson<T>(string jsonStr)
        {
            Activator.CreateInstance<T>();
            using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr));
            return (T)new DataContractJsonSerializer(typeof(T)).ReadObject((Stream)stream);
        }

        public static string JSON1<SendData>(List<SendData> vals)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SendData));
                foreach (SendData val in vals)
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    dataContractJsonSerializer.WriteObject((Stream)memoryStream, (object?)val);
                    stringBuilder.Append(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return stringBuilder.ToString();
        }

        public static JObject ExtractObj(string jsonObject)
        {
            return ExtractObj(JObject.Parse(jsonObject));
        }

        public static JObject ExtractObj(JObject job)
        {
            foreach (KeyValuePair<string, JToken> item in job)
            {
                JToken value = item.Value;
                if (value.Type == JTokenType.String)
                {
                    string json = value.ToString();
                    if (IsJson(json))
                    {
                        JToken jToken = JToken.Parse(json);
                        if (jToken.Type == JTokenType.Object)
                        {
                            job[item.Key] = ExtractObj((JObject)jToken);
                        }
                        else if (jToken.Type == JTokenType.Array)
                        {
                            job[item.Key] = ExtractArr((JArray)jToken);
                        }
                    }
                }
                else if (value.Type == JTokenType.Object)
                {
                    job[item.Key] = ExtractObj((JObject)value);
                }
                else if (value.Type == JTokenType.Array)
                {
                    job[item.Key] = ExtractArr((JArray)value);
                }
            }
            return job;
        }

        public static JArray ExtractArr(string jsonArr)
        {
            return ExtractArr(JArray.Parse(jsonArr));
        }

        public static JArray ExtractArr(JArray jArr)
        {
            for (int i = 0; i < jArr.Count; i++)
            {
                JToken jToken = jArr[i];
                if (jToken.Type == JTokenType.String)
                {
                    string json = jToken.ToString();
                    if (IsJson(json))
                    {
                        JToken jToken2 = JToken.Parse(json);
                        if (jToken2.Type == JTokenType.Array)
                        {
                            jArr[i] = ExtractArr((JArray)jToken2);
                        }
                        else if (jToken2.Type == JTokenType.Object)
                        {
                            jArr[i] = ExtractObj((JObject)jToken2);
                        }
                    }
                }
                else if (jToken.Type == JTokenType.Array)
                {
                    jArr[i] = ExtractArr((JArray)jToken);
                }
                else if (jToken.Type == JTokenType.Object)
                {
                    jArr[i] = ExtractObj((JObject)jToken);
                }
            }
            return jArr;
        }

        public static JToken ExtractAll(string json)
        {
            try
            {
                return ExtractAll(JToken.Parse(json));
            }
            catch
            {
                throw new Exception("不是有效的JToken对象");
            }
        }

        public static JToken ExtractAll(JToken jToken)
        {
            if (jToken.Type == JTokenType.String)
            {
                jToken = JToken.Parse(jToken.ToString());
            }
            if (jToken.Type == JTokenType.Object)
            {
                return ExtractObj((JObject)jToken);
            }
            if (jToken.Type == JTokenType.Array)
            {
                return ExtractArr((JArray)jToken);
            }
            throw new Exception("暂不支持提取[" + jToken.Type.ToString() + "]类型");
        }

        public static bool IsJson(string json)
        {
            json = json.Trim();
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }
            char c = json.First();
            if (c == '{' || c == '[')
            {
                return true;
            }
            return false;
        }

        public static string JsonFormat(string str)
        {
            try
            {
                JsonSerializer jsonSerializer = new JsonSerializer();
                JsonTextReader reader = new JsonTextReader(new StringReader(str));
                object obj = jsonSerializer.Deserialize(reader);
                if (obj != null)
                {
                    StringWriter stringWriter = new StringWriter();
                    JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter)
                    {
                        Formatting = Formatting.Indented,
                        Indentation = 4,
                        IndentChar = ' '
                    };
                    jsonSerializer.Serialize(jsonWriter, obj);
                    return stringWriter.ToString();
                }
                return str;
            }
            catch
            {
                return str;
            }
        }
    }
}
