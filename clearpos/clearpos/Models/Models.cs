using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clearpos.Models
{
    //class Models
    //{
        public class datatableModel
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
            public double Tax { get; set; }
            public string altupc1 { get; set; }
            public string altupc2 { get; set; }
            public string altupc3 { get; set; }
            public string altupc4 { get; set; }
            public string altupc5 { get; set; }
            public decimal Deposit { get; set; }
        }
        class FullNameProductModel
        {
            public string pname { get; set; }
            public string pdesc { get; set; }
            public string upc { get; set; }
            public string sku { get; set; }
            public decimal Price { get; set; }
            public string uom { get; set; }
            public int pack { get; set; }
            public string pcat { get; set; }
            public string pcat1 { get; set; }
            public string pcat2 { get; set; }
            public string country { get; set; }
            public string region { get; set; }
        }
        public class Root
        {
            public string RESP { get; set; }
            public string ID { get; set; }
            public string DESC { get; set; }
            public string UNIT { get; set; }
            public string TAXCODE { get; set; }
            public string PRICE { get; set; }
            public string RETAIL { get; set; }
            public string DEPPRICE { get; set; }
            public string DEPTAXCODE { get; set; }
            public string DEPTNUM { get; set; }
            public string COST { get; set; }
            public string COUNTRY { get; set; }
            public string LDESC { get; set; }
            public string UPC { get; set; }
            public string AVAILABLE { get; set; }
            public string DEPTNO { get; set; }
        }
        public class Root1
        {
            public string RESP { get; set; }
            public string DEPT1 { get; set; }
            public string DEPT6 { get; set; }
            public string DEPT8 { get; set; }
            public string DEPT10 { get; set; }
            public string DEPT11 { get; set; }
            public string DEPT13 { get; set; }
            public string DEPT14 { get; set; }
            public string DEPT15 { get; set; }
            public string DEPT16 { get; set; }
            public string DEPT20 { get; set; }
            public string DEPT40 { get; set; }
            public string DEPT45 { get; set; }
            public string DEPT50 { get; set; }
            public string DEPT55 { get; set; }
            public string DEPT75 { get; set; }
            public string DEPT80 { get; set; }
            public string DEPT85 { get; set; }
            public string DEPT90 { get; set; }
            public string DEPT95 { get; set; }
            public string DEPT98 { get; set; }
            public string DEPT99 { get; set; }
            public string DEPT100 { get; set; }
            public string DEPT101 { get; set; }
            public string DEPT102 { get; set; }
            public string DEPT103 { get; set; }
            public string DEPT104 { get; set; }
            public string DEPT105 { get; set; }
            public string DEPT106 { get; set; }
            public string DEPT107 { get; set; }
            public string DEPT108 { get; set; }
            public string DEPT109 { get; set; }
            public string DEPT110 { get; set; }
            public string DEPT111 { get; set; }
            public string DEPT112 { get; set; }
            public string DEPT113 { get; set; }
            public string DEPT114 { get; set; }
            public string DEPT115 { get; set; }
            public string DEPT116 { get; set; }
            public string DEPT117 { get; set; }
            public string DEPT120 { get; set; }
            public string DEPT125 { get; set; }
            public string DEPT130 { get; set; }
            public string DEPT131 { get; set; }
            public string DEPT132 { get; set; }
            public string DEPT133 { get; set; }
            public string DEPT134 { get; set; }
            public string DEPT135 { get; set; }
            public string DEPT136 { get; set; }
            public string DEPT137 { get; set; }
            public string DEPT138 { get; set; }
            public string DEPT139 { get; set; }
            public string DEPT140 { get; set; }
            public string DEPT141 { get; set; }
            public string DEPT145 { get; set; }
            public string DEPT150 { get; set; }
            public string DEPT155 { get; set; }
            public string DEPT156 { get; set; }
            public string DEPT157 { get; set; }
            public string DEPT158 { get; set; }
            public string DEPT160 { get; set; }
            public string DEPT165 { get; set; }
            public string DEPT170 { get; set; }
            public string DEPT175 { get; set; }
            public string DEPT180 { get; set; }
            public string DEPT200 { get; set; }
            public string DEPT205 { get; set; }
            public string DEPT210 { get; set; }
            public string DEPT211 { get; set; }
            public string DEPT212 { get; set; }
            public string DEPT213 { get; set; }
            public string DEPT214 { get; set; }
            public string DEPT215 { get; set; }
            public string DEPT216 { get; set; }
            public string DEPT220 { get; set; }
            public string DEPT225 { get; set; }
            public string DEPT230 { get; set; }
            public string DEPT260 { get; set; }
            public string DEPT270 { get; set; }
            public string DEPT290 { get; set; }
            public string DEPT295 { get; set; }
            public string DEPT300 { get; set; }
            public string DEPT380 { get; set; }
            public string DEPT400 { get; set; }
            public string DEPT900 { get; set; }
            public string DEPT901 { get; set; }
            public string DEPT975 { get; set; }
            public string DEPT976 { get; set; }
            public string DEPT980 { get; set; }
            public string DEPT999 { get; set; }
        }
    //}
}
