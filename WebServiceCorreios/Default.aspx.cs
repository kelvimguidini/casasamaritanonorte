using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TestandoWebServiceCorreios.Lib;

namespace TestandoWebServiceCorreios
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            //instanciamos a classe
            ConsultaCorreios consulta = new ConsultaCorreios();
            //populamos a classe
            consulta.CdEmpresa    = string.Empty;
            consulta.SenhaEmpresa = string.Empty;
            consulta.CdServico = CodServico.SedexSemContrato;
            consulta.CepOrigem = "72916251";
            consulta.CepDestino = "09720971";
            consulta.PesoEncomenda = "2";
            consulta.FormatoEncomenda = FormatoEncomenda.CaixaPacote;
            consulta.Altura = 15;
            consulta.Largura = 30;
            consulta.Diametro = 40;
            consulta.Comprimento = 20;
            consulta.MaoPropria = "N";
            consulta.AvisoRecebimento = "N";
            consulta.ValorDeclarado = 0;
            TextBox1.Text = consulta.ValorServico();
        }
    }
}
