using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Budget.Migrations
{
    public partial class AddBudgetClassFKMigration_BudgetDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tenant_Id",
                schema: "Budget",
                table: "BudgetClasses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetClasses_Tenant_Id",
                schema: "Budget",
                table: "BudgetClasses",
                column: "Tenant_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetClasses_Tenants_Tenant_Id",
                schema: "Budget",
                table: "BudgetClasses",
                column: "Tenant_Id",
                principalSchema: "Tenants",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetClasses_Tenants_Tenant_Id",
                schema: "Budget",
                table: "BudgetClasses");

            migrationBuilder.DropIndex(
                name: "IX_BudgetClasses_Tenant_Id",
                schema: "Budget",
                table: "BudgetClasses");

            migrationBuilder.DropColumn(
                name: "Tenant_Id",
                schema: "Budget",
                table: "BudgetClasses");
        }
    }
}
