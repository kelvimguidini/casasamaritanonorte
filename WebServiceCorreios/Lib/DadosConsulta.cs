using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestandoWebServiceCorreios.Lib
{
    public class ConsultaCorreios
    {   //dados da empresa caso ela tenha contrato com os correios;
        private string cdEmpresa;

        public string CdEmpresa
        {
            get { return cdEmpresa; }
            set { cdEmpresa = value; }
        }
        private string senhaEmpresa;

        public string SenhaEmpresa
        {
            get { return senhaEmpresa; }
            set { senhaEmpresa = value; }
        }
        //codigo do tipo de frete - ex: 40010 = sedex
        private CodServico cdServico;

        public CodServico CdServico
        {
            get { return cdServico; }
            set { cdServico = value; }
        }
        //cep origem da encomenda
        private string cepOrigem;

        public string CepOrigem
        {
            get { return cepOrigem; }
            set { cepOrigem = value; }
        }
        //cep destino da encomenda
        private string cepDestino;

        public string CepDestino
        {
            get { return cepDestino; }
            set { cepDestino = value; }
        }
        //peso da encomenda
        private string pesoEncomenda;

        public string PesoEncomenda
        {
            get { return pesoEncomenda; }
            set { pesoEncomenda = value; }
        }

        private FormatoEncomenda formatoEncomenda;

        public FormatoEncomenda FormatoEncomenda
        {
            get { return formatoEncomenda; }
            set { formatoEncomenda = value; }
        }

        private decimal comprimento;

        public decimal Comprimento
        {
            get { return comprimento; }
            set { comprimento = value; }
        }
        private decimal altura;

        public decimal Altura
        {
            get { return altura; }
            set { altura = value; }
        }
        private decimal largura;

        public decimal Largura
        {
            get { return largura; }
            set { largura = value; }
        }
        private decimal diametro;

        public decimal Diametro
        {
            get { return diametro; }
            set { diametro = value; }
        }
        private string maoPropria;

        public string MaoPropria
        {
            get { return maoPropria; }
            set { maoPropria = value; }
        }
        private decimal valorDeclarado;

        public decimal ValorDeclarado
        {
            get { return valorDeclarado; }
            set { valorDeclarado = value; }
        }
        private string avisoRecebimento;

        public string AvisoRecebimento
        {
            get { return avisoRecebimento; }
            set { avisoRecebimento = value; }
        }

        public string ValorServico()
        { 
            //Instanciamos o WebService
            Correios.CalcPrecoPrazoWS webCorreios = new Correios.CalcPrecoPrazoWS();

            //Efetuamos a Requisição
            Correios.cResultado retornoCorreios = webCorreios.CalcPrecoPrazo(cdEmpresa, senhaEmpresa, Convert.ToInt32(cdServico).ToString(),
                                                                             cepOrigem, cepDestino, pesoEncomenda,
                                                                             Convert.ToInt32(formatoEncomenda), comprimento, altura,
                                                                             largura, diametro, maoPropria,
                                                                             valorDeclarado, avisoRecebimento);

            //verificamos o retorno

            if (retornoCorreios.Servicos.Length > 0)
            {
                if (retornoCorreios.Servicos[0].Erro == "0")
                {
                    return "R$" + retornoCorreios.Servicos[0].Valor;
                }
                else
                {
                    return retornoCorreios.Servicos[0].MsgErro;
                }
            }
            else
            {
                return "Não foi possível efetuar a consulta";
            }
        }

    }
    public enum CodServico
    {
        PacSemContrato = 41106,
        SedexSemContrato = 40010,
        SedexCobrarSemContrato = 40045,
        SedexCobrarComContrato = 40126,
        Sedex10SemContrato = 40215,
        SedexHojeSemContrato = 40290,
        SedexComContrato = 40096
    }
    public enum FormatoEncomenda
    {
        CaixaPacote = 1,
        RoloPrisma = 2
    }


}
