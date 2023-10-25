using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQL_Csharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string GetCSharpDataType(string sqlDataType)
        {
            switch (sqlDataType.ToLower())
            {
                case "bigint":
                    return "long";
                case "binary":
                    return "byte[]";
                case "bit":
                    return "bool";
                case "char":
                    return "string";
                case "date":
                case "datetime":
                case "datetime2":
                    return "DateTime";
                case "datetimeoffset":
                    return "DateTimeOffset";
                case "decimal":
                case "numeric":
                case "money":
                case "smallmoney":
                    return "decimal";
                case "float":
                    return "double";
                case "image":
                    return "byte[]";
                case "int":
                    return "int";
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "text":
                case "varchar":
                    return "string";
                case "real":
                    return "float";
                case "smalldatetime":
                    return "DateTime";
                case "smallint":
                    return "short";
                case "time":
                    return "TimeSpan";
                case "timestamp":
                    return "DateTime";
                case "tinyint":
                    return "byte";
                case "uniqueidentifier":
                    return "Guid";
                case "varbinary":
                    return "byte[]";
                default:
                    return "UNKNOWN_" + sqlDataType;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "server=.;database=TraversalDB;integrated security = true;"; // Veritabanı bağlantı dizesini buraya ekleyin
            string tableName = txtTableName.Text.Trim(); // Kullanıcının girdiği tablo adını alın

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Lütfen bir tablo adı girin.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT TOP 0 * FROM {tableName}"; // Sadece sütun şemasını almak için hiçbir veri alınmayacak

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
                    {
                        DataTable schemaTable = reader.GetSchemaTable();

                        if (schemaTable != null)
                        {
                            richTextBoxOutput.Clear(); // Önceki verileri temizleyin

                            foreach (DataRow row in schemaTable.Rows)
                            {
                                string columnName = row["ColumnName"].ToString();
                                string sqlDataType = row["DataTypeName"].ToString();
                                string cSharpDataType = GetCSharpDataType(sqlDataType);

                                richTextBoxOutput.AppendText($"public {cSharpDataType} {columnName} {{ get; set; }}" + Environment.NewLine);
                            }
                        }
                    }
                }
            }
        }
    }
}