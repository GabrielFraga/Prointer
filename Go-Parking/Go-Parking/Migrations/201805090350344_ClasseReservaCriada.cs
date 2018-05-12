namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClasseReservaCriada : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vaga", "VeiculoId", "dbo.Veiculo");
            DropIndex("dbo.Vaga", new[] { "VeiculoId" });
            CreateTable(
                "dbo.Reserva",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Entrada = c.DateTimeOffset(precision: 7),
                        Saida = c.DateTimeOffset(precision: 7),
                        Vaga_Id = c.Int(),
                        Veiculo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vaga", t => t.Vaga_Id)
                .ForeignKey("dbo.Veiculo", t => t.Veiculo_Id)
                .Index(t => t.Vaga_Id)
                .Index(t => t.Veiculo_Id);
            
            AddColumn("dbo.Vaga", "Nome", c => c.String());
            DropColumn("dbo.Vaga", "UserId");
            DropColumn("dbo.Vaga", "Modelo");
            DropColumn("dbo.Vaga", "placa");
            DropColumn("dbo.Vaga", "VeiculoId");
            DropColumn("dbo.Vaga", "HoraEntrada");
            DropColumn("dbo.Vaga", "HoraSaida");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vaga", "HoraSaida", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Vaga", "HoraEntrada", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Vaga", "VeiculoId", c => c.Int(nullable: false));
            AddColumn("dbo.Vaga", "placa", c => c.String());
            AddColumn("dbo.Vaga", "Modelo", c => c.String());
            AddColumn("dbo.Vaga", "UserId", c => c.String());
            DropForeignKey("dbo.Reserva", "Veiculo_Id", "dbo.Veiculo");
            DropForeignKey("dbo.Reserva", "Vaga_Id", "dbo.Vaga");
            DropIndex("dbo.Reserva", new[] { "Veiculo_Id" });
            DropIndex("dbo.Reserva", new[] { "Vaga_Id" });
            DropColumn("dbo.Vaga", "Nome");
            DropTable("dbo.Reserva");
            CreateIndex("dbo.Vaga", "VeiculoId");
            AddForeignKey("dbo.Vaga", "VeiculoId", "dbo.Veiculo", "Id", cascadeDelete: true);
        }
    }
}
