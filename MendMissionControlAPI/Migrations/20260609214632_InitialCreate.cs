using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MendMissionControlAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetritosOrbitais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CodigoCatalogo = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    MassaKg = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    TamanhoCm = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    VelocidadeKmh = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    AltitudeKm = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    NivelRisco = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataIdentificacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Removido = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DET", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipamentosEspaciais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CodigoIdentificacao = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    DataLancamento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    TipoEquipamento = table.Column<string>(type: "NVARCHAR2(21)", maxLength: 21, nullable: false),
                    CapacidadeCombustivel = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PossuiLaserAblacao = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    PossuiGarrasCaptura = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    Operador = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EstaAtivo = table.Column<bool>(type: "NUMBER(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EQP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MissoesRemocao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NomeMissao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DetritoOrbitalId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NaveLimpezaOrbitalId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MetodoRemocao = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Status = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataPlanejada = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Observacao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MIS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MIS_DET",
                        column: x => x.DetritoOrbitalId,
                        principalTable: "DetritosOrbitais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MIS_EQP",
                        column: x => x.NaveLimpezaOrbitalId,
                        principalTable: "EquipamentosEspaciais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Telemetrias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NaveLimpezaOrbitalId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PosicaoX = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PosicaoY = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    VelocidadeKmh = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    NivelBateria = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    TemperaturaInterna = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TEL_EQP",
                        column: x => x.NaveLimpezaOrbitalId,
                        principalTable: "EquipamentosEspaciais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MendCredits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MissaoRemocaoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Cliente = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ValorCredito = table.Column<decimal>(type: "DECIMAL(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CRE_MIS",
                        column: x => x.MissaoRemocaoId,
                        principalTable: "MissoesRemocao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });



            CriarSequenciasETriggersOracle(migrationBuilder);
            migrationBuilder.Sql(@"
                INSERT INTO ""EquipamentosEspaciais""
                (""Id"", ""CapacidadeCombustivel"", ""CodigoIdentificacao"", ""DataLancamento"", ""Nome"", ""PossuiGarrasCaptura"", ""PossuiLaserAblacao"", ""TipoEquipamento"")
                VALUES
                (1, 1000, 'MEND-ORION-001', TIMESTAMP '2026-06-09 00:00:00', 'Orion', 1, 1, 'NAVE_LIMPEZA_ORBITAL')
            ");

            migrationBuilder.CreateIndex(
                name: "IX_DET_COD",
                table: "DetritosOrbitais",
                column: "CodigoCatalogo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EQP_COD",
                table: "EquipamentosEspaciais",
                column: "CodigoIdentificacao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CRE_MIS",
                table: "MendCredits",
                column: "MissaoRemocaoId");

            migrationBuilder.CreateIndex(
                name: "IX_MIS_DET",
                table: "MissoesRemocao",
                column: "DetritoOrbitalId");

            migrationBuilder.CreateIndex(
                name: "IX_MIS_EQP",
                table: "MissoesRemocao",
                column: "NaveLimpezaOrbitalId");

            migrationBuilder.CreateIndex(
                name: "IX_TEL_EQP",
                table: "Telemetrias",
                column: "NaveLimpezaOrbitalId");
        }


        private static void CriarSequenciasETriggersOracle(MigrationBuilder migrationBuilder)
        {
            CriarSequenciaETrigger(migrationBuilder, "SEQ_DET_ORB", "TRG_DET_ORB_BI", "DetritosOrbitais");
            CriarSequenciaETrigger(migrationBuilder, "SEQ_EQP_ESP", "TRG_EQP_ESP_BI", "EquipamentosEspaciais");
            CriarSequenciaETrigger(migrationBuilder, "SEQ_MIS_REM", "TRG_MIS_REM_BI", "MissoesRemocao");
            CriarSequenciaETrigger(migrationBuilder, "SEQ_TEL", "TRG_TEL_BI", "Telemetrias");
            CriarSequenciaETrigger(migrationBuilder, "SEQ_MEND_CRED", "TRG_MEND_CRED_BI", "MendCredits");
        }

        private static void CriarSequenciaETrigger(MigrationBuilder migrationBuilder, string sequenceName, string triggerName, string tableName)
        {
            migrationBuilder.Sql($"CREATE SEQUENCE \"{sequenceName}\" START WITH 100 INCREMENT BY 1 NOCACHE");

            migrationBuilder.Sql($"CREATE OR REPLACE TRIGGER \"{triggerName}\" BEFORE INSERT ON \"{tableName}\" FOR EACH ROW BEGIN IF :NEW.\"Id\" IS NULL THEN SELECT \"{sequenceName}\".NEXTVAL INTO :NEW.\"Id\" FROM DUAL; END IF; END;");
        }

        private static void RemoverSequenciasETriggersOracle(MigrationBuilder migrationBuilder)
        {
            RemoverTriggerESequencia(migrationBuilder, "TRG_MEND_CRED_BI", "SEQ_MEND_CRED");
            RemoverTriggerESequencia(migrationBuilder, "TRG_TEL_BI", "SEQ_TEL");
            RemoverTriggerESequencia(migrationBuilder, "TRG_MIS_REM_BI", "SEQ_MIS_REM");
            RemoverTriggerESequencia(migrationBuilder, "TRG_EQP_ESP_BI", "SEQ_EQP_ESP");
            RemoverTriggerESequencia(migrationBuilder, "TRG_DET_ORB_BI", "SEQ_DET_ORB");
        }

        private static void RemoverTriggerESequencia(MigrationBuilder migrationBuilder, string triggerName, string sequenceName)
        {
            migrationBuilder.Sql($"BEGIN EXECUTE IMMEDIATE 'DROP TRIGGER \"{triggerName}\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            migrationBuilder.Sql($"BEGIN EXECUTE IMMEDIATE 'DROP SEQUENCE \"{sequenceName}\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            RemoverSequenciasETriggersOracle(migrationBuilder);

            migrationBuilder.DropTable(
                name: "MendCredits");

            migrationBuilder.DropTable(
                name: "Telemetrias");

            migrationBuilder.DropTable(
                name: "MissoesRemocao");

            migrationBuilder.DropTable(
                name: "DetritosOrbitais");

            migrationBuilder.DropTable(
                name: "EquipamentosEspaciais");
        }
    }
}
