namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correcao3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reserva", "Vagas_Id", "dbo.Vaga");
            DropForeignKey("dbo.Reserva", "Veiculos_Id", "dbo.Veiculo");
            DropIndex("dbo.Reserva", new[] { "Vagas_Id" });
            DropIndex("dbo.Reserva", new[] { "Veiculos_Id" });
            RenameColumn(table: "dbo.Reserva", name: "Vagas_Id", newName: "VagaId");
            RenameColumn(table: "dbo.Reserva", name: "Veiculos_Id", newName: "VeiculoId");
            AlterColumn("dbo.Reserva", "VagaId", c => c.Int(nullable: false));
            AlterColumn("dbo.Reserva", "VeiculoId", c => c.Int(nullable: false));
            CreateIndex("dbo.Reserva", "VagaId");
            CreateIndex("dbo.Reserva", "VeiculoId");
            AddForeignKey("dbo.Reserva", "VagaId", "dbo.Vaga", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Reserva", "VeiculoId", "dbo.Veiculo", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reserva", "VeiculoId", "dbo.Veiculo");
            DropForeignKey("dbo.Reserva", "VagaId", "dbo.Vaga");
            DropIndex("dbo.Reserva", new[] { "VeiculoId" });
            DropIndex("dbo.Reserva", new[] { "VagaId" });
            AlterColumn("dbo.Reserva", "VeiculoId", c => c.Int());
            AlterColumn("dbo.Reserva", "VagaId", c => c.Int());
            RenameColumn(table: "dbo.Reserva", name: "VeiculoId", newName: "Veiculos_Id");
            RenameColumn(table: "dbo.Reserva", name: "VagaId", newName: "Vagas_Id");
            CreateIndex("dbo.Reserva", "Veiculos_Id");
            CreateIndex("dbo.Reserva", "Vagas_Id");
            AddForeignKey("dbo.Reserva", "Veiculos_Id", "dbo.Veiculo", "Id");
            AddForeignKey("dbo.Reserva", "Vagas_Id", "dbo.Vaga", "Id");
        }
    }
}
