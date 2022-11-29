using clearpos.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace clearpos
{
    class Products
    {
        string DeveloperId = ConfigurationManager.AppSettings["DeveloperId"];
        string folderPath = ConfigurationManager.AppSettings["BaseDirectory"];
        string altupcassku = ConfigurationManager.AppSettings["Altupcassku"];
        string deposit = ConfigurationManager.AppSettings["deposit"];
        private int list;
        private int LastitemID;
        public Products(int StoreId, decimal tax, string IP, int Port, string ID, string PassWord)
        {
            try
            {
                Console.WriteLine("Generating Product file for  CLEAR StoreID  " + StoreId + " ..........");
                CLEARsetting(StoreId, tax, IP, Port, ID, PassWord);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " CLEAR" + StoreId);
            }
        }
        public List<Root> ProductsXmlGen(int StoreId, decimal tax, string IP, int Port, string ID, string PassWord)
        {
            ArrayList Plistt = new ArrayList()
            {

            };
            List<Root> productList = new List<Root>();
            string LastitemID = "";
            for (int i = 0; i < 65; i++)
            {
                try
                {
                    var sb = new StringBuilder();
                    sb.Append($"\u0002");
                    sb.Append($"<ACCESSID>" + ID + "</ACCESSID>");  //CLRWEB
                    sb.Append($"<ACCESSPASS>" + PassWord + "</ACCESSPASS>"); //1234
                    sb.Append($"<SYSTEM>RET</SYSTEM>");
                    sb.Append($"<REQUEST>ITEMLIST</REQUEST>");
                    sb.Append($"<ID>" + LastitemID + "</ID>");
                    sb.Append($"<DETAIL></DETAIL>");
                    sb.Append($"\u0003");
                    int PORT_NO = Port;  //7999
                    string hOST = IP;  //50.68.46.1  //24.87.154.145
                    TcpClient client = new TcpClient(hOST, PORT_NO);
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    string ab = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                    var ab1 = ab.Replace("\u0002", "<root>").Replace("\u0003", "</root>");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(ab1);
                    var json = JsonConvert.SerializeXmlNode(doc);
                    var datalist = JsonConvert.DeserializeObject<ProductList>(json);
                    productList.Add(datalist.root);
                    client.Close();
                    ArrayList personList = new ArrayList();
                    personList.Add(json);
                    for (int j = 0; j < personList.Count; j++)
                    {
                        string pelist = (string)personList[j];
                        string ItemID = Regex.Replace(pelist, @"[root]", "");
                        ItemID = Regex.Replace(ItemID, @"[\ ]", "");
                        ItemID = Regex.Replace(ItemID, @"[ "" ]", "");
                        ItemID = Regex.Replace(ItemID, @"[{]", "");
                        ItemID = Regex.Replace(ItemID, @"[}]", "");
                        ItemID = Regex.Replace(ItemID, @"[A-Z]", "");
                        ItemID = Regex.Replace(ItemID, @"[:]", "");

                        string[] lastid = ItemID.Split(',');
                        for (int k = 0; k < lastid.Count(); k++)
                        {
                            if (lastid[k].ToString() != "")
                            {
                                Plistt.Add(lastid[k].ToString());
                            }
                        }
                        LastitemID = lastid.Last();
                    }
                }
                catch (Exception ex)
                {
                    (new clsEmail()).sendEmail(DeveloperId, "", "", "Error in ClearPos@" + "StoreID: " + StoreId + " " + DateTime.UtcNow + " GMT", ex.Message + "<br/>" + ex.StackTrace);
                    Console.WriteLine(ex.Message);

                }
            }
            var TotalPlist = Plistt;
            List<Root> productList1 = new List<Root>();
            for (int i = 0; i < TotalPlist.Count; i++)
            {
                try
                {
                    ArrayList a = new ArrayList()
                    {

                    };
                    var sb = new StringBuilder();
                    sb.Append($"\u0002");
                    sb.Append($"<ACCESSID>" + ID + "</ACCESSID>");
                    sb.Append($"<ACCESSPASS>" + PassWord + "</ACCESSPASS>");
                    sb.Append($"<SYSTEM>RET</SYSTEM>");
                    sb.Append($"<REQUEST>ITEM</REQUEST>");
                    sb.Append($"<ID>" + TotalPlist[i].ToString() + " </ID>");
                    sb.Append($"<DETAIL></DETAIL>");
                    sb.Append($"\u0003");
                    int PORT_NO = Port;
                    string hOST = IP;   // 50.68.46.1  //24.87.154.145
                    TcpClient client = new TcpClient(hOST, PORT_NO);
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    string ab = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                    var ab1 = ab.Replace("\u0002", "<root>").Replace("\u0003", "</root>");
                    ab1 = Regex.Replace(ab1, @"[&]", "");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(ab1);
                    var json = JsonConvert.SerializeXmlNode(doc);
                    var datalist = JsonConvert.DeserializeObject<productList1>(json);
                    productList1.Add(datalist.root);
                    client.Close();
                }
                catch (Exception ex)
                {
                    (new clsEmail()).sendEmail(DeveloperId, "", "", "Error in ClearPos@" + "StoreID: "+ StoreId+ " " + DateTime.UtcNow + " GMT", ex.Message + "<br/>" + ex.StackTrace);
                    Console.WriteLine(ex.Message);
                }
            }
            return productList1;
        }
        public List<Root1> categorygeneration(int StoreId, decimal tax, string IP, int Port, string ID, string PassWord)
        {
            List<Root1> productList2 = new List<Root1>();

            for (int i = 0; i < 1; i++)
            {
                try
                {
                    ArrayList a = new ArrayList()
                    {

                    };
                    var sb = new StringBuilder();
                    sb.Append($"\u0002");
                    sb.Append($"<ACCESSID>" + ID + "</ACCESSID>");
                    sb.Append($"<ACCESSPASS>" + PassWord + "</ACCESSPASS>");
                    sb.Append($"<SYSTEM>RET</SYSTEM>");
                    sb.Append($"<REQUEST>DEPTLIST</REQUEST>");
                    sb.Append($"<DETAIL></DETAIL>");
                    sb.Append($"\u0003");
                    int PORT_NO = Port;
                    string hOST = IP;   //  50.68.46.1  //24.87.154.145
                    TcpClient client = new TcpClient(hOST, PORT_NO);
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    string ab = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                    var ab1 = ab.Replace("\u0002", "<root>").Replace("\u0003", "</root>");
                    ab1 = Regex.Replace(ab1, @"[&]", "");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(ab1);
                    var json = JsonConvert.SerializeXmlNode(doc);
                    var datalist1 = JsonConvert.DeserializeObject<productList2>(json);
                    productList2.Add(datalist1.root);
                    client.Close();
                }
                catch (Exception ex)
                {
                    (new clsEmail()).sendEmail(DeveloperId, "", "", "Error in ClearPos@" + "StoreID: " + StoreId + " " + DateTime.UtcNow + " GMT", ex.Message + "<br/>" + ex.StackTrace);
                    Console.WriteLine(ex.Message);
                }
            }
            return productList2;
        }
        public void CLEARsetting(int StoreId, decimal tax, string IP, int Port, string ID, String PassWord)
        {
            try
            {
                var productList1 = ProductsXmlGen(StoreId, tax, IP, Port, ID, PassWord);
                var productList2 = categorygeneration(StoreId, tax, IP, Port, ID, PassWord);

                var catList = new Dictionary<string, string>();

                var props = productList2[0].GetType().GetProperties().ToList();

                for (int i = 0; i < props.Count; i++)
                {
                    var propName = props[i].Name;
                    var propValue = props[i].GetValue(productList2[0]);
                    if (propValue != null)
                    {
                        catList.Add(propName.Replace("DEPT", ""), propValue.ToString());
                    }
                }
                foreach (var item in productList1)
                {
                    var catName = catList.FirstOrDefault(f => f.Key == item.DEPTNUM).Value;
                    if (catName != null)
                    {
                        item.DEPTNUM = catName;
                    }
                }
                var BaseUrl = "";

                BaseUrl = ConfigurationManager.AppSettings.Get("BaseDirectory");
                List<datatableModel> pf = new List<datatableModel>();
                List<FullNameProductModel> pf1 = new List<FullNameProductModel>();

                foreach (var item in productList1)
                {
                    datatableModel pdf = new datatableModel();
                    FullNameProductModel fdf = new FullNameProductModel();
                    pdf.StoreID = StoreId;
                    pdf.upc = "#" + item.UPC;
                    fdf.upc = "#" + item.UPC;
                    if (altupcassku.Contains(StoreId.ToString()))
                    {
                        pdf.sku = "#" + item.ID;
                        fdf.sku = "#" + item.UPC;
                    }
                    if (!string.IsNullOrEmpty(item.AVAILABLE))
                    {
                        pdf.Qty = Convert.ToInt32(item.AVAILABLE) > 0 ? Convert.ToInt32(item.AVAILABLE) : 0;
                    }
                    pdf.pack = 1;
                    fdf.pack = 1;
                    pdf.uom = item.UNIT;
                    fdf.uom = item.UNIT;
                    pdf.StoreProductName = item.DESC;
                    pdf.StoreDescription = item.LDESC;
                    fdf.pname = item.DESC;
                    fdf.pdesc = item.LDESC;

                    if (!string.IsNullOrEmpty(item.PRICE))
                    {
                        pdf.Price = Convert.ToDecimal(item.PRICE);
                        fdf.Price = Convert.ToDecimal(item.PRICE);
                    }
                    if (pdf.Price < 0 || fdf.Price < 0)
                    {
                        continue;
                    }
                    pdf.Start = "";
                    pdf.End = "";
                    pdf.Tax = Convert.ToDouble(tax);
                    if (!altupcassku.Contains(StoreId.ToString()))
                    {
                        pdf.altupc1 = "";
                    }
                    pdf.altupc2 = "";
                    pdf.altupc3 = "";
                    pdf.altupc4 = "";
                    fdf.pcat = item.DEPTNUM;
                    fdf.pcat1 = "";
                    fdf.pcat2 = "";
                    fdf.country = "";
                    fdf.region = "";
                    if (deposit.Contains(StoreId.ToString()))
                    {
                        pdf.Deposit = Convert.ToDecimal(0.10);
                    }
                    else
                    {
                        pdf.Deposit = 0;
                    }
                    
                    if (fdf.pcat == "FOODSNACKSPOP" || fdf.pcat == "CARDS" || fdf.pcat == "CRAFT CIDER" || fdf.pcat == "LITTER" || fdf.pcat == "LITTER RETURNS" || fdf.pcat == "MISCELLANEOUS")
                    {
                        pdf.Tax = 0.12;
                    }
                    if (fdf.pcat == "CIDERS" || fdf.pcat == "" || fdf.pcat == "CRAFT CIDER" || fdf.pcat == "DOMESTIC 24PK" || fdf.pcat == "DOMESTIC 4-6PK" || fdf.pcat == "DOMESTIC 8-15PK"
                        || fdf.pcat == "IMPORT PREMIUM" || fdf.pcat == "IMPORT SINGLES" || fdf.pcat == "LOCAL CRAFT 4PK" || fdf.pcat == "LOCAL CRAFT 6PK" || fdf.pcat == "LOCAL CRAFT 8-15PK"
                        || fdf.pcat == "LOCAL SINGLES" || fdf.pcat == "CRAFT" || fdf.pcat == "BEER 8 PK")
                    {
                        if (Regex.IsMatch(pdf.uom.ToString(), @"((\d+PK))"))
                        {
                            var abc = Regex.Match(pdf.uom.ToString(), @"((\d+PK))")?.Value;
                            string digits = new String(abc.TakeWhile(Char.IsDigit).ToArray());

                            pdf.Deposit = (Int32.Parse(digits) * Convert.ToDecimal(0.10));
                        }
                    }
                    if (pdf.upc != "#" || fdf.upc != "#")
                    {
                        pf.Add(pdf);
                        pf1.Add(fdf);
                        pf = pf.GroupBy(x => x.upc).Select(y => y.FirstOrDefault()).ToList();
                        pf1 = pf1.GroupBy(x => x.upc).Select(y => y.FirstOrDefault()).ToList();
                    }
                }
                GenerateCSV.GenerateCSVFile(pf, "PRODUCT", StoreId, folderPath);
                GenerateCSV.GenerateCSVFile(pf1, "FULLNAME", StoreId, folderPath);
                Console.WriteLine("Generated Product file for  CLEAR StoreID  " + StoreId + " ..........");
                Console.WriteLine("Generated FullName file for  CLEAR StoreID  " + StoreId + " ..........");
            }
            catch (Exception ex)
            {
                (new clsEmail()).sendEmail(DeveloperId, "", "", "Error in ClearPos@" + "StoreID: " + StoreId + " " + DateTime.UtcNow + " GMT", ex.Message + "<br/>" + ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }
        public class ProductList
        {
            public Root root { get; set; }
        }
        public class productList1
        {
            internal Root1 root1;
            public Root root { get; set; }
        }
        public class productList2
        {
            internal Root1 root1;
            public Root1 root { get; set; }
        }
        internal interface IRestResponse
        {
            string Content { get; set; }
        }
        internal class GenerateCSV
        {
            public static string GenerateCSVFile(DataTable dt, string Name, int StoreId)
            {
                StringBuilder sb = new StringBuilder();
                try
                {
                    int count = 1;
                    int totalColumns = dt.Columns.Count;
                    foreach (DataColumn dr in dt.Columns)
                    {
                        sb.Append(dr.ColumnName);

                        if (count != totalColumns)
                        {
                            sb.Append(",");
                        }
                        count++;
                    }
                    sb.AppendLine();

                    string value = String.Empty;
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int x = 0; x < totalColumns; x++)
                        {
                            value = dr[x].ToString();

                            if (value.Contains(",") || value.Contains("\""))
                            {
                                value = '"' + value.Replace("\"", "\"\"") + '"';
                            }
                            sb.Append(value);

                            if (x != (totalColumns - 1))
                            {
                                sb.Append(",");
                            }
                        }
                        sb.AppendLine();
                    }
                    if (!Directory.Exists("Upload\\"))
                    {
                        Directory.CreateDirectory("Upload\\");
                    }
                    string filename = Name + StoreId + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
                    File.WriteAllText("Upload\\" + filename, sb.ToString());
                    return filename;
                }
                catch (Exception)
                {
                    // Do something
                }
                return "";
            }
            public static string GenerateCSVFile<T>(IList<T> list, string Name, int StoreId, string BaseUrl)
            {

                if (list == null || list.Count == 0) return "";
                if (!Directory.Exists(BaseUrl + "\\" + StoreId + "\\Upload\\"))
                {
                    Directory.CreateDirectory(BaseUrl + "\\" + StoreId + "\\Upload\\");
                }
                string filename = Name + StoreId + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
                string fcname = BaseUrl + "\\" + StoreId + "\\Upload\\" + filename;
                // Console.WriteLine("Generating " + filename + " ........");
                //File.WriteAllText(BaseUrl + "\\" + StoreId + "\\Upload\\" + filename, csvData.ToString());
                // return filename;

                //get type from 0th member
                Type t = list[0].GetType();
                string newLine = Environment.NewLine;

                using (var sw = new StreamWriter(fcname))
                {
                    //make a new instance of the class name we figured out to get its props
                    object o = Activator.CreateInstance(t);
                    //gets all properties
                    PropertyInfo[] props = o.GetType().GetProperties();

                    //foreach of the properties in class above, write out properties
                    //this is the header row
                    foreach (PropertyInfo pi in props)
                    {
                        sw.Write(pi.Name + ",");
                    }
                    sw.Write(newLine);

                    //this acts as datarow
                    foreach (T item in list)
                    {
                        //this acts as datacolumn
                        foreach (PropertyInfo pi in props)
                        {
                            //this is the row+col intersection (the value)
                            string whatToWrite =
                                Convert.ToString(item.GetType()
                                                     .GetProperty(pi.Name)
                                                     .GetValue(item, null))
                                    .Replace(',', ' ') + ',';

                            sw.Write(whatToWrite);

                        }
                        sw.Write(newLine);
                    }
                    return filename;
                }
            }
            public static string GenerateCSVFile(List<ProductModel> list, string Name, int StoreId, string BaseUrl)
            {
                StringBuilder csvData = null;
                try
                {
                    csvData = new StringBuilder();
                    //Get the properties for type T for the headers
                    PropertyInfo[] propInfos = typeof(ProductModel).GetProperties();
                    for (int i = 0; i <= propInfos.Length - 1; i++)
                    {
                        csvData.Append(propInfos[i].Name);
                        if (i < propInfos.Length - 1)
                        {
                            csvData.Append(",");
                        }
                    }
                    csvData.AppendLine();

                    //Loop through the collection, then the properties and add the values
                    for (int i = 0; i <= list.Count - 1; i++)
                    {
                        ProductModel item = list[i];
                        for (int j = 0; j <= propInfos.Length - 1; j++)
                        {
                            object csvProperty = item.GetType().GetProperty(propInfos[j].Name).GetValue(item, null);
                            if (csvProperty != null)
                            {
                                string value = csvProperty.ToString();
                                //Check if the value contans a comma and place it in quotes if so
                                //if (value.Contains(","))
                                //{
                                //    value = string.Concat("\"", value, "\"");
                                //}
                                ////Replace any \r or \n special characters from a new line with a space
                                //if (value.Contains("\r"))
                                //{
                                //    value = value.Replace("\r", " ");
                                //}
                                //if (value.Contains("\n"))
                                //{
                                //    value = value.Replace("\n", " ");
                                //}

                                value = '"' + value.Replace("\"", "\"\"") + '"';

                                csvData.Append(value);
                            }
                            if (j < propInfos.Length - 1)
                            {
                                csvData.Append(",");
                            }
                        }
                        csvData.AppendLine();
                    }

                    if (!Directory.Exists(BaseUrl + "\\" + StoreId + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(BaseUrl + "\\" + StoreId + "\\Upload\\");
                    }
                    string filename = Name + StoreId + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
                    File.WriteAllText(BaseUrl + "\\" + StoreId + "\\Upload\\" + filename, csvData.ToString());
                    return filename;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                    // Do something
                }
            }
            public static string GenerateCSVFile(List<ProductModelEcrs> list, string Name, int StoreId, string BaseUrl)
            {
                StringBuilder csvData = null;
                try
                {
                    csvData = new StringBuilder();
                    //Get the properties for type T for the headers
                    PropertyInfo[] propInfos = typeof(ProductModelEcrs).GetProperties();
                    for (int i = 0; i <= propInfos.Length - 1; i++)
                    {
                        csvData.Append(propInfos[i].Name);
                        if (i < propInfos.Length - 1)
                        {
                            csvData.Append(",");
                        }
                    }
                    csvData.AppendLine();

                    //Loop through the collection, then the properties and add the values
                    for (int i = 0; i <= list.Count - 1; i++)
                    {
                        ProductModelEcrs item = list[i];
                        for (int j = 0; j <= propInfos.Length - 1; j++)
                        {
                            object csvProperty = item.GetType().GetProperty(propInfos[j].Name).GetValue(item, null);
                            if (csvProperty != null)
                            {
                                string value = csvProperty.ToString();
                                //Check if the value contans a comma and place it in quotes if so
                                if (value.Contains(","))
                                {
                                    value = string.Concat("\"", value, "\"");
                                }
                                //Replace any \r or \n special characters from a new line with a space
                                if (value.Contains("\r"))
                                {
                                    value = value.Replace("\r", " ");
                                }
                                if (value.Contains("\n"))
                                {
                                    value = value.Replace("\n", " ");
                                }
                                csvData.Append(value);
                            }
                            if (j < propInfos.Length - 1)
                            {
                                csvData.Append(",");
                            }
                        }
                        csvData.AppendLine();
                    }

                    if (!Directory.Exists(BaseUrl + "\\" + StoreId + "\\Upload\\")) { Directory.CreateDirectory(BaseUrl + "\\" + StoreId + "\\Upload\\"); }
                    string filename = Name + StoreId + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
                    File.WriteAllText(BaseUrl + "\\" + StoreId + "\\Upload\\" + filename, csvData.ToString());
                    return filename;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                    // Do something
                }

            }
            public static string GenerateCSVFile(List<ProductModelShopKeep> list, string Name, int StoreId, string BaseUrl)
            {
                StringBuilder csvData = null;
                try
                {
                    csvData = new StringBuilder();
                    //Get the properties for type T for the headers
                    PropertyInfo[] propInfos = typeof(ProductModelEcrs).GetProperties();
                    for (int i = 0; i <= propInfos.Length - 1; i++)
                    {
                        csvData.Append(propInfos[i].Name);
                        if (i < propInfos.Length - 1)
                        {
                            csvData.Append(",");
                        }
                    }
                    csvData.AppendLine();

                    //Loop through the collection, then the properties and add the values
                    for (int i = 0; i <= list.Count - 1; i++)
                    {
                        ProductModelShopKeep item = list[i];
                        for (int j = 0; j <= propInfos.Length - 1; j++)
                        {
                            object csvProperty = item.GetType().GetProperty(propInfos[j].Name).GetValue(item, null);
                            if (csvProperty != null)
                            {
                                string value = csvProperty.ToString();
                                //Check if the value contans a comma and place it in quotes if so
                                if (value.Contains(","))
                                {
                                    value = string.Concat("\"", value, "\"");
                                }
                                //Replace any \r or \n special characters from a new line with a space
                                if (value.Contains("\r"))
                                {
                                    value = value.Replace("\r", " ");
                                }
                                if (value.Contains("\n"))
                                {
                                    value = value.Replace("\n", " ");
                                }
                                csvData.Append(value);
                            }
                            if (j < propInfos.Length - 1)
                            {
                                csvData.Append(",");
                            }
                        }
                        csvData.AppendLine();
                    }

                    if (!Directory.Exists(BaseUrl + "\\" + StoreId + "\\Upload\\")) { Directory.CreateDirectory(BaseUrl + "\\" + StoreId + "\\Upload\\"); }
                    string filename = Name + StoreId + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
                    File.WriteAllText(BaseUrl + "\\" + StoreId + "\\Upload\\" + filename, csvData.ToString());
                    return filename;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                    // Do something
                }

            }
            public static void ToCSV(DataTable dtDataTable, string strFilePath)
            {
                StreamWriter sw = new StreamWriter(strFilePath, false);
                //headers  
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    sw.Write(dtDataTable.Columns[i]);
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dtDataTable.Rows)
                {
                    for (int i = 0; i < dtDataTable.Columns.Count; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            string value = dr[i].ToString();
                            if (value.Contains(','))
                            {
                                value = String.Format("\"{0}\"", value);
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(dr[i].ToString());
                            }
                        }
                        if (i < dtDataTable.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }




        }

        internal class ProductModel
        {
            public int StoreID { get; set; }
            public string upc { get; set; }
            public decimal Qty { get; set; }
            public string sku { get; set; }
            public int pack { get; set; }
            public string uom { get; set; }
            public string StoreProductName { get; set; }
            public string StoreDescription { get; set; }
            public decimal Price { get; set; }
            public decimal sprice { get; set; }
            public string Start { get; set; }
            public string End { get; set; }
            public decimal Tax { get; set; }
            public string altupc1 { get; set; }
            public string altupc2 { get; set; }
            public string altupc3 { get; set; }
            public string altupc4 { get; set; }
            public string altupc5 { get; set; }
        }
        internal class ProductModelShopKeep
        {

            public int StoreID { get; set; }
            public string upc { get; set; }
            public decimal Qty { get; set; }
            public string sku { get; set; }
            public int pack { get; set; }
            public string StoreProductName { get; set; }
            public string StoreDescription { get; set; }
            public string size { get; set; }
            //public string uom { get; set; }
            public decimal price { get; set; }
            public decimal sprice { get; set; }
            public string Start { get; set; }
            public string End { get; set; }
            public decimal Tax { get; set; }

        }

        internal class ProductModelEcrs
        {

            public int StoreID { get; set; }
            public string upc { get; set; }
            public decimal Qty { get; set; }
            public string sku { get; set; }
            public int pack { get; set; }
            public string StoreProductName { get; set; }
            public string StoreDescription { get; set; }
            public string size { get; set; }
            public string uom { get; set; }
            public decimal price { get; set; }
            public decimal sprice { get; set; }
            public string Start { get; set; }
            public string End { get; set; }
            public decimal Tax { get; set; }

        }
    }
}
