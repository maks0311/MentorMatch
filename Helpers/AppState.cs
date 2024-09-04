using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mentor.Data;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace Mentor
{
    [Serializable()]
    public class AppState
    {
        public UserModel UserInfo { get; set; }
        public LessonModel LessonInfo { get; set; }
        public TutorModel TutorInfo { get; set; }
        public bool IsAdmin { get; set; }
        public int TabIndex { get; set; } = 0;
        public String CRUD { get; set; }
        public List<AppStateParams> Params { get; set; }

        public AppState()
        {
            try
            {
                this.UserInfo = new UserModel();
                this.LessonInfo = new LessonModel();
                this.TutorInfo = new TutorModel();
                this.IsAdmin = false;
                this.CRUD = string.Empty;
                this.Params = new List<AppStateParams>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        public void SetParam(string key, object val)
        {
            try
            {
                if (Params.Exists(x => x.Key.ToUpper() == key.ToUpper()))
                {
                    foreach (var item in Params)
                    {
                        if (item.Key.ToUpper() == key.ToUpper())
                        {
                            item.Val = val.ToString();
                            return;
                        }
                    }
                }
                else
                {
                    Params.Add(new AppStateParams()
                    {
                        Key = key,
                        Val = val.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        public void SetParamAsString(string key, string val)
        {
            SetParam(key, val);
        }

        public void SetParamAsInteger(string key, int val)
        {
            SetParam(key, val);
        }

        public void SetParamAsDecimal(string key, decimal val)
        {
            SetParam(key, val);
        }

        public void SetParamAsDateTime(string key, DateTime val)
        {
            SetParam(key, val);
        }

        public object GetParam(string key)
        {
            object retVal = null;

            try
            {
                if (Params.Exists(x => x.Key.ToUpper() == key.ToUpper()))
                {
                    foreach (var item in Params)
                    {
                        if (item.Key.ToUpper() == key.ToUpper())
                        {
                            retVal = item.Val;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }

            return retVal;
        }

        public string GetParamAsString(string key, string defVal)
        {
            try
            {
                if (ParameterExists(key))
                    return GetParam(key).ToString();
                else
                    return defVal;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
                return defVal;
            }
        }

        public int GetParamAsInteger(string key, int defVal)
        {
            try
            {
                if (ParameterExists(key))
                {
                    int.TryParse(GetParam(key).ToString(), out int retVal);
                    return retVal;
                }
                else
                {
                    return defVal;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
                return defVal;
            }
        }

        public decimal GetParamAsDecimal(string key, decimal defVal)
        {
            try
            {
                if (ParameterExists(key))
                {
                    Decimal.TryParse(GetParam(key).ToString().Replace(",", "."), out Decimal retVal);
                    return retVal;
                }
                else
                {
                    return defVal;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
                return defVal;
            }
        }

        public DateTime GetParamAsDateTime(string key, DateTime defVal)
        {
            try
            {
                if (ParameterExists(key))
                {
                    DateTime.TryParse(GetParam(key).ToString(), out DateTime retVal);
                    return retVal;
                }
                else
                {
                    return defVal;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
                return defVal;
            }
        }

        private bool ParameterExists(string key)
        {
            try
            {
                foreach (var item in Params)
                {
                    if (item.Key.ToUpper() == key.ToUpper())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void DeleteParam(string key)
        {
            AppStateParams param = Params.Find(args => args.Key == key);
            Params.Remove(param);
        }
    }
}
