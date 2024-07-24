using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Data;
namespace paginaWeb
{
    public class cls_convertir_JSON
    {


        public DataTable ConvertJArrayToDataTable(JArray jArray)
        {

            var dataTable = new DataTable();

            if (jArray.Count == 0)
                return dataTable;

            foreach (var jToken in jArray.First.Children<JProperty>())
            {
                dataTable.Columns.Add(jToken.Name, typeof(string));
            }

            foreach (var jObject in jArray.Children<JObject>())
            {
                var dataRow = dataTable.NewRow();
                foreach (var jProperty in jObject.Properties())
                {
                    dataRow[jProperty.Name] = jProperty.Value.ToString();
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

    }
}