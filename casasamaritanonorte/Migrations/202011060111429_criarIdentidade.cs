namespace casasamaritanonorte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class criarIdentidade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Foto", "Album_ID", "dbo.Album");
            DropForeignKey("dbo.CaptadorCampanha", "Captador_ID", "dbo.Captador");
            DropForeignKey("dbo.CaptadorCampanha", "Campanha_ID", "dbo.Campanha");
            DropForeignKey("dbo.Evento", "Album_ID", "dbo.Album");
            DropForeignKey("dbo.Pagamento", "Campanha_ID", "dbo.Campanha");
            DropForeignKey("dbo.Pagamento", "Captador_ID", "dbo.Captador");
            DropIndex("dbo.Foto", new[] { "Album_ID" });
            DropIndex("dbo.Evento", new[] { "Album_ID" });
            DropIndex("dbo.Pagamento", new[] { "Campanha_ID" });
            DropIndex("dbo.Pagamento", new[] { "Captador_ID" });
            DropIndex("dbo.CaptadorCampanha", new[] { "Captador_ID" });
            DropIndex("dbo.CaptadorCampanha", new[] { "Campanha_ID" });
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            DropTable("dbo.Album");
            DropTable("dbo.Foto");
            DropTable("dbo.Campanha");
            DropTable("dbo.Captador");
            DropTable("dbo.Contato");
            DropTable("dbo.Evento");
            DropTable("dbo.Pagamento");
            DropTable("dbo.CaptadorCampanha");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CaptadorCampanha",
                c => new
                    {
                        Captador_ID = c.Int(nullable: false),
                        Campanha_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Captador_ID, t.Campanha_ID });
            
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
                "dbo.Foto",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Caminho = c.String(),
                        Album_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Album",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            CreateIndex("dbo.CaptadorCampanha", "Campanha_ID");
            CreateIndex("dbo.CaptadorCampanha", "Captador_ID");
            CreateIndex("dbo.Pagamento", "Captador_ID");
            CreateIndex("dbo.Pagamento", "Campanha_ID");
            CreateIndex("dbo.Evento", "Album_ID");
            CreateIndex("dbo.Foto", "Album_ID");
            AddForeignKey("dbo.Pagamento", "Captador_ID", "dbo.Captador", "ID");
            AddForeignKey("dbo.Pagamento", "Campanha_ID", "dbo.Campanha", "ID");
            AddForeignKey("dbo.Evento", "Album_ID", "dbo.Album", "ID");
            AddForeignKey("dbo.CaptadorCampanha", "Campanha_ID", "dbo.Campanha", "ID", cascadeDelete: true);
            AddForeignKey("dbo.CaptadorCampanha", "Captador_ID", "dbo.Captador", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Foto", "Album_ID", "dbo.Album", "ID");
        }
    }
}
