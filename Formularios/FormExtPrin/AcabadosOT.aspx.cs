using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class AcabadosOT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar si la variable de sesión "Id_OT" existe
            if (Session["Id_OT"] != null)
            {
                // Obtener el valor de la variable de sesión "Id_OT" y asignarlo a una variable local
                string id = Session["Id_OT"].ToString();

                // Puedes usar la variable "id" en este formulario según tus necesidades
                // Por ejemplo, para configurar el SqlDataSource
                SqlDataSource1.SelectParameters["Id_OT"].DefaultValue = id;      

                // Ahora puedes utilizar el valor "id" en el SqlDataSource o en cualquier otro lugar necesario en este formulario.
            }

        }

        protected void lnkSelectRow_Click(object sender, EventArgs e)
        {
            int rowIndex = GetSelectedRowIndex(sender);
            if (rowIndex == -1)
            {
                return; // No se seleccionó ninguna fila
            }

            DataGrid dataGrid1 = DataGrid1; // Reemplaza 'DataGrid1' con el nombre de tu primer DataGrid
            DataGrid dataGrid2 = DataGrid2; // Reemplaza 'DataGrid2' con el nombre de tu segundo DataGrid

            DeselectAllRows(dataGrid1);
            DeselectAllRows(dataGrid2);

            // Deselecciona todas las filas previamente seleccionadas en ambos DataGrids
            foreach (DataGridItem item in dataGrid1.Items)
            {
                item.CssClass = "";
            }
            foreach (DataGridItem item in dataGrid2.Items)
            {
                item.CssClass = "";
            }

            DataGridItem selectedRow1 = dataGrid1.Items[rowIndex];
            string grupoObjetoparaAcabado = selectedRow1.Cells[1].Text;

            foreach (DataGridItem item in dataGrid2.Items)
            {
                if (item.Cells[0].Text == grupoObjetoparaAcabado)
                {
                    item.CssClass = "selected-roww";

                }
            }

            selectedRow1.CssClass = "selected-row";


            string descripcionGrupo = GetDescripcionGrupo(grupoObjetoparaAcabado);
            ShowDescripcionGrupo(descripcionGrupo);

            // Obtén y muestra la descripción del acabado en el TextArea1
            string descripcionAcabado = GetDescripcionAcabado(descripcionGrupo);
            TextArea1.InnerText = descripcionAcabado;
        }

        // Función para obtener el índice de la fila seleccionada
        private int GetSelectedRowIndex(object sender)
        {
            LinkButton lnkSelectRow = (LinkButton)sender;
            return Convert.ToInt32(lnkSelectRow.CommandArgument);
        }

        // Función para deseleccionar todas las filas en un DataGrid
        private void DeselectAllRows(DataGrid dataGrid)
        {
            foreach (DataGridItem item in dataGrid.Items)
            {
                item.CssClass = "";
            }
        }

        // Función para obtener la descripción del grupo
        private string GetDescripcionGrupo(string grupoObjetoparaAcabado)
        {
            string sqlQuery = "SELECT Descripcion_Grupo FROM tblAcabado A " +
                    "JOIN tblOTAcabados OT ON A.ID_Acabado = OT.ID_Acabado " +
                    "JOIN tblGrupodeAcabado GA ON A.ID_GrupoAcabado = GA.ID_GrupoAcabado " +
                    "JOIN tblGrupoObjetoParaAcabado GOA ON GOA.ID_GrupoObjetoparaAcabado = OT.ID_GrupoObjetoparaAcabado " +
                    "WHERE OT.Id_OT = @Id_OT AND GOA.GrupoObjetoparaAcabado = @grupoObjetoparaAcabado";

            using (SqlConnection conn = new SqlConnection("Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password"))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Id_OT", Session["Id_OT"].ToString());
                cmd.Parameters.AddWithValue("@grupoObjetoparaAcabado", grupoObjetoparaAcabado);
                conn.Open();

                
            

            var descripcionGrupo = cmd.ExecuteScalar()?.ToString();
                conn.Close();

                // Asigna el valor al Label y hazlo visible
                Label3.Text = grupoObjetoparaAcabado;
                Label3.Visible = true;

                return descripcionGrupo;
            }

        }

        // Función para mostrar la descripción del grupo en Label1
        private void ShowDescripcionGrupo(string descripcionGrupo)
        {
            Label1.Text = descripcionGrupo ?? "Descripción no encontrada";
            Label1.Visible = true;
        }

        // Función para obtener la descripción del acabado
        private string GetDescripcionAcabado(string descripcionGrupo)
        {
            string sqlQuery = "SELECT A.Descripcion_Acabado FROM tblAcabado A " +
                    "JOIN tblOTAcabados OT ON A.ID_Acabado = OT.ID_Acabado " +
                    "JOIN tblGrupodeAcabado GA ON A.ID_GrupoAcabado = GA.ID_GrupoAcabado " +
                    "JOIN tblGrupoObjetoParaAcabado GOA ON GOA.ID_GrupoObjetoparaAcabado = OT.ID_GrupoObjetoparaAcabado " +
                    "WHERE OT.Id_OT = @Id_OT AND GOA.GrupoObjetoparaAcabado = @descripcionGrupo " +
                    "UNION " +
                    "SELECT Descripcion_Acabado FROM tblAcabado " +
                    "WHERE ID_GrupoAcabado = (SELECT ID_GrupoAcabado FROM tblGrupodeAcabado WHERE Descripcion_Grupo = @descripcionGrupo AND Activo = '1')";


            using (SqlConnection conn = new SqlConnection("Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password"))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Id_OT", Session["Id_OT"].ToString());
                cmd.Parameters.AddWithValue("@descripcionGrupo", descripcionGrupo);
                conn.Open();

               
            


            var resultText = new StringBuilder();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resultText.Append(reader["Descripcion_Acabado"].ToString());
                        resultText.Append(Environment.NewLine); // Agregar un salto de línea entre los resultados
                    }
                }

                conn.Close();

                return resultText.ToString();
            }
        }








    }
}