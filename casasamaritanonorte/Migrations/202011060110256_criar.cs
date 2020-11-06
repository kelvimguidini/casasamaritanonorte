namespace casasamaritanonorte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class criar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Album",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Foto",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Caminho = c.String(),
                        Album_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Album", t => t.Album_ID)
                .Index(t => t.Album_ID);
            
            CreateTable(
                "dbo.Campanha",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        VideoStr = c.String(),
                        fotoStr = c.String(),
                        Nome = c.String(),
                        Descricao = c.String(),
                        Mensagem = c.String(),
                        Doacao = c.Boolean(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Peso = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Altura = c.Int(nullable: false),
                        Largura = c.Int(nullable: false),
                        Diametro = c.Int(nullable: false),
                        Comprimento = c.Int(nullable: false),
                        TelaInicial = c.Boolean(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Captador",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Email = c.String(),
                        DataNascimento = c.DateTime(nullable: false),
                        CPF = c.String(),
                        Telefone = c.String(),
                        Banco = c.String(),
                        Agencia = c.String(),
                        Conta = c.String(),
                        tipoConta = c.Int(nullable: false),
                        Multirao = c.Boolean(nullable: false),
                        Integral = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Contato",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Email = c.String(),
                        Assunto = c.String(),
                        Mensagem = c.String(),
                        Data = c.DateTime(nullable: false),
                        Lido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Evento",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CaminhoCapa = c.String(),
                        Titulo = c.String(),
                        DataEvento = c.DateTime(nullable: false),
                        Caption = c.String(),
                        Texto = c.String(),
                        DataPublicacao = c.DateTime(nullable: false),
                        Album_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Album", t => t.Album_ID)
                .Index(t => t.Album_ID);
            
            CreateTable(
                "dbo.Pagamento",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Email = c.String(),
                        Telefone = c.String(),
                        DDD = c.String(),
                        CPF = c.String(),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantidade = c.Int(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Frete = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrazoEntrega = c.Int(nullable: false),
                        SituacaoEntrega = c.String(),
                        CodigoTrasacao = c.String(),
                        TipoEntrega = c.Int(nullable: false),
                        Cep = c.String(),
                        Logradouro = c.String(),
                        Numero = c.String(),
                        Complemento = c.String(),
                        UF = c.String(),
                        Bairro = c.String(),
                        Cidade = c.String(),
                        Data = c.DateTime(nullable: false),
                        PrimeiraVisualizacao = c.Boolean(nullable: false),
                        ComissaoPaga = c.Boolean(nullable: false),
                        Campanha_ID = c.Int(),
                        Captador_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Campanha", t => t.Campanha_ID)
                .ForeignKey("dbo.Captador", t => t.Captador_ID)
                .Index(t => t.Campanha_ID)
                .Index(t => t.Captador_ID);
            
            CreateTable(
                "dbo.CaptadorCampanha",
                c => new
                    {
                        Captador_ID = c.Int(nullable: false),
                        Campanha_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Captador_ID, t.Campanha_ID })
                .ForeignKey("dbo.Captador", t => t.Captador_ID, cascadeDelete: true)
                .ForeignKey("dbo.Campanha", t => t.Campanha_ID, cascadeDelete: true)
                .Index(t => t.Captador_ID)
                .Index(t => t.Campanha_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pagamento", "Captador_ID", "dbo.Captador");
            DropForeignKey("dbo.Pagamento", "Campanha_ID", "dbo.Campanha");
            DropForeignKey("dbo.Evento", "Album_ID", "dbo.Album");
            DropForeignKey("dbo.CaptadorCampanha", "Campanha_ID", "dbo.Campanha");
            DropForeignKey("dbo.CaptadorCampanha", "Captador_ID", "dbo.Captador");
            DropForeignKey("dbo.Foto", "Album_ID", "dbo.Album");
            DropIndex("dbo.CaptadorCampanha", new[] { "Campanha_ID" });
            DropIndex("dbo.CaptadorCampanha", new[] { "Captador_ID" });
            DropIndex("dbo.Pagamento", new[] { "Captador_ID" });
            DropIndex("dbo.Pagamento", new[] { "Campanha_ID" });
            DropIndex("dbo.Evento", new[] { "Album_ID" });
            DropIndex("dbo.Foto", new[] { "Album_ID" });
            DropTable("dbo.CaptadorCampanha");
            DropTable("dbo.Pagamento");
            DropTable("dbo.Evento");
            DropTable("dbo.Contato");
            DropTable("dbo.Captador");
            DropTable("dbo.Campanha");
            DropTable("dbo.Foto");
            DropTable("dbo.Album");
        }
    }
}
