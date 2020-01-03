using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanMed.Migrations
{
    public partial class CreateBd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cep",
                columns: table => new
                {
                    CepId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logradouro = table.Column<string>(maxLength: 200, nullable: false),
                    Bairro = table.Column<string>(maxLength: 200, nullable: false),
                    Cidade = table.Column<string>(maxLength: 200, nullable: false),
                    UF = table.Column<string>(maxLength: 200, nullable: false),
                    CEP = table.Column<string>(maxLength: 10, nullable: false),
                    Complemento = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cep", x => x.CepId);
                });

            migrationBuilder.CreateTable(
                name: "Conselhos",
                columns: table => new
                {
                    ConselhoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conselhos", x => x.ConselhoId);
                });

            migrationBuilder.CreateTable(
                name: "Especialidades",
                columns: table => new
                {
                    EspecialidadeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidades", x => x.EspecialidadeId);
                });

            migrationBuilder.CreateTable(
                name: "GrupoFaturamentos",
                columns: table => new
                {
                    GrupoFaturamentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: false),
                    Tipo = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoFaturamentos", x => x.GrupoFaturamentoId);
                });

            migrationBuilder.CreateTable(
                name: "NiveisAcessos",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NiveisAcessos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecursoAgendamentos",
                columns: table => new
                {
                    RecursoAgendamentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: false),
                    Sigla = table.Column<string>(maxLength: 4, nullable: false),
                    Tipo = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecursoAgendamentos", x => x.RecursoAgendamentoId);
                });

            migrationBuilder.CreateTable(
                name: "Setores",
                columns: table => new
                {
                    SetorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(maxLength: 160, nullable: false),
                    TipoSetor = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setores", x => x.SetorId);
                });

            migrationBuilder.CreateTable(
                name: "TipoPrestadores",
                columns: table => new
                {
                    TipoPrestadorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoPrestadores", x => x.TipoPrestadorId);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    EmpresaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeFantasia = table.Column<string>(nullable: false),
                    RazaoSocial = table.Column<string>(nullable: false),
                    CNPJ = table.Column<string>(nullable: false),
                    InscricaoEstadual = table.Column<string>(nullable: true),
                    InscricaoMunicipal = table.Column<string>(nullable: true),
                    CNES = table.Column<string>(nullable: true),
                    CepId = table.Column<int>(nullable: true),
                    Numero = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    Unidade = table.Column<bool>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Logo = table.Column<string>(nullable: true),
                    DtCadastro = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.EmpresaId);
                    table.ForeignKey(
                        name: "FK_Empresas_Cep_CepId",
                        column: x => x.CepId,
                        principalTable: "Cep",
                        principalColumn: "CepId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Procedimentos",
                columns: table => new
                {
                    ProcedimentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: false),
                    IdadeMaxima = table.Column<int>(nullable: true),
                    IdadeMinima = table.Column<int>(nullable: true),
                    Sexo = table.Column<string>(nullable: false),
                    GrupoFaturamentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedimentos", x => x.ProcedimentoId);
                    table.ForeignKey(
                        name: "FK_Procedimentos_GrupoFaturamentos_GrupoFaturamentoId",
                        column: x => x.GrupoFaturamentoId,
                        principalTable: "GrupoFaturamentos",
                        principalColumn: "GrupoFaturamentoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_NiveisAcessos_RoleId",
                        column: x => x.RoleId,
                        principalTable: "NiveisAcessos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exames",
                columns: table => new
                {
                    ExameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(maxLength: 160, nullable: false),
                    Mnemonico = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    DiaEntrega = table.Column<int>(nullable: true),
                    Sexo = table.Column<string>(nullable: false),
                    PreparoExame = table.Column<string>(type: "text", nullable: true),
                    SetorId = table.Column<int>(nullable: true),
                    EspecialidadeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exames", x => x.ExameId);
                    table.ForeignKey(
                        name: "FK_Exames_Especialidades_EspecialidadeId",
                        column: x => x.EspecialidadeId,
                        principalTable: "Especialidades",
                        principalColumn: "EspecialidadeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exames_Setores_SetorId",
                        column: x => x.SetorId,
                        principalTable: "Setores",
                        principalColumn: "SetorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "date", nullable: false),
                    CPF = table.Column<string>(maxLength: 14, nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Observacao = table.Column<string>(nullable: true),
                    SetorId = table.Column<int>(nullable: true),
                    CepId = table.Column<int>(nullable: true),
                    Numero = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    DtCadastro = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Cep_CepId",
                        column: x => x.CepId,
                        principalTable: "Cep",
                        principalColumn: "CepId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuarios_Setores_SetorId",
                        column: x => x.SetorId,
                        principalTable: "Setores",
                        principalColumn: "SetorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prestadores",
                columns: table => new
                {
                    PrestadorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(maxLength: 160, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "Date", nullable: false),
                    CPF = table.Column<string>(maxLength: 16, nullable: false),
                    Sexo = table.Column<string>(nullable: true),
                    NumeroCrm = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Telefone = table.Column<string>(maxLength: 20, nullable: true),
                    TipoPrestadorId = table.Column<int>(nullable: false),
                    ConselhoId = table.Column<int>(nullable: false),
                    Numero = table.Column<string>(maxLength: 5, nullable: true),
                    Email = table.Column<string>(maxLength: 160, nullable: true),
                    CepId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestadores", x => x.PrestadorId);
                    table.ForeignKey(
                        name: "FK_Prestadores_Cep_CepId",
                        column: x => x.CepId,
                        principalTable: "Cep",
                        principalColumn: "CepId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prestadores_Conselhos_ConselhoId",
                        column: x => x.ConselhoId,
                        principalTable: "Conselhos",
                        principalColumn: "ConselhoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prestadores_TipoPrestadores_TipoPrestadorId",
                        column: x => x.TipoPrestadorId,
                        principalTable: "TipoPrestadores",
                        principalColumn: "TipoPrestadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemAgendamentos",
                columns: table => new
                {
                    ItemAgendamentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: false),
                    Tipo = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    ExameId = table.Column<int>(nullable: true),
                    RecursoAgendamentoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemAgendamentos", x => x.ItemAgendamentoId);
                    table.ForeignKey(
                        name: "FK_ItemAgendamentos_Exames_ExameId",
                        column: x => x.ExameId,
                        principalTable: "Exames",
                        principalColumn: "ExameId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemAgendamentos_RecursoAgendamentos_RecursoAgendamentoId",
                        column: x => x.RecursoAgendamentoId,
                        principalTable: "RecursoAgendamentos",
                        principalColumn: "RecursoAgendamentoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_NiveisAcessos_RoleId",
                        column: x => x.RoleId,
                        principalTable: "NiveisAcessos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SobreUsuarios",
                columns: table => new
                {
                    SobreUsuarioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Formacao = table.Column<string>(nullable: true),
                    Foto = table.Column<string>(nullable: true),
                    UsuarioId = table.Column<string>(nullable: true),
                    CepId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SobreUsuarios", x => x.SobreUsuarioId);
                    table.ForeignKey(
                        name: "FK_SobreUsuarios_Cep_CepId",
                        column: x => x.CepId,
                        principalTable: "Cep",
                        principalColumn: "CepId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SobreUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosEmpresas",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(nullable: false),
                    EmpresaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosEmpresas", x => new { x.UsuarioId, x.EmpresaId });
                    table.ForeignKey(
                        name: "FK_UsuariosEmpresas_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "EmpresaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosEmpresas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgendasMedicas",
                columns: table => new
                {
                    AgendaMedicaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataAgenda = table.Column<DateTime>(nullable: false),
                    RecursoAgendamentoId = table.Column<int>(nullable: true),
                    PrestadorId = table.Column<int>(nullable: true),
                    HoraInicio = table.Column<DateTime>(nullable: false),
                    HoraFim = table.Column<DateTime>(nullable: false),
                    StatusAgenda = table.Column<bool>(nullable: false),
                    DataLiberacao = table.Column<DateTime>(nullable: false),
                    QtAtendimento = table.Column<int>(nullable: false),
                    QtEncaixe = table.Column<int>(nullable: false),
                    DtCadastro = table.Column<DateTime>(nullable: false),
                    Observacao = table.Column<string>(nullable: true),
                    ThemeColor = table.Column<string>(nullable: true),
                    UsuarioId = table.Column<string>(nullable: true),
                    EmpresaId = table.Column<int>(nullable: true),
                    QtTempoMedio = table.Column<TimeSpan>(nullable: false),
                    SetorId = table.Column<int>(nullable: true),
                    TipoAgenda = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendasMedicas", x => x.AgendaMedicaId);
                    table.ForeignKey(
                        name: "FK_AgendasMedicas_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "EmpresaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AgendasMedicas_Prestadores_PrestadorId",
                        column: x => x.PrestadorId,
                        principalTable: "Prestadores",
                        principalColumn: "PrestadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AgendasMedicas_RecursoAgendamentos_RecursoAgendamentoId",
                        column: x => x.RecursoAgendamentoId,
                        principalTable: "RecursoAgendamentos",
                        principalColumn: "RecursoAgendamentoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AgendasMedicas_Setores_SetorId",
                        column: x => x.SetorId,
                        principalTable: "Setores",
                        principalColumn: "SetorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AgendasMedicas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrestadoresEspecialidades",
                columns: table => new
                {
                    PrestadorId = table.Column<int>(nullable: false),
                    EspecialidadeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestadoresEspecialidades", x => new { x.PrestadorId, x.EspecialidadeId });
                    table.ForeignKey(
                        name: "FK_PrestadoresEspecialidades_Especialidades_EspecialidadeId",
                        column: x => x.EspecialidadeId,
                        principalTable: "Especialidades",
                        principalColumn: "EspecialidadeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrestadoresEspecialidades_Prestadores_PrestadorId",
                        column: x => x.PrestadorId,
                        principalTable: "Prestadores",
                        principalColumn: "PrestadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemAgendamentoPrestadores",
                columns: table => new
                {
                    PrestadorId = table.Column<int>(nullable: false),
                    ItemAgendamentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemAgendamentoPrestadores", x => new { x.PrestadorId, x.ItemAgendamentoId });
                    table.ForeignKey(
                        name: "FK_ItemAgendamentoPrestadores_ItemAgendamentos_ItemAgendamentoId",
                        column: x => x.ItemAgendamentoId,
                        principalTable: "ItemAgendamentos",
                        principalColumn: "ItemAgendamentoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemAgendamentoPrestadores_Prestadores_PrestadorId",
                        column: x => x.PrestadorId,
                        principalTable: "Prestadores",
                        principalColumn: "PrestadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensAgendasMedica",
                columns: table => new
                {
                    ItemAgendamentoId = table.Column<int>(nullable: false),
                    AgendaMedicaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensAgendasMedica", x => new { x.ItemAgendamentoId, x.AgendaMedicaId });
                    table.ForeignKey(
                        name: "FK_ItensAgendasMedica_AgendasMedicas_AgendaMedicaId",
                        column: x => x.AgendaMedicaId,
                        principalTable: "AgendasMedicas",
                        principalColumn: "AgendaMedicaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensAgendasMedica_ItemAgendamentos_ItemAgendamentoId",
                        column: x => x.ItemAgendamentoId,
                        principalTable: "ItemAgendamentos",
                        principalColumn: "ItemAgendamentoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agendamentos",
                columns: table => new
                {
                    AgendamentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgendaMedicaId = table.Column<int>(nullable: false),
                    HoraAgenda = table.Column<TimeSpan>(nullable: false),
                    PacienteId = table.Column<int>(nullable: false),
                    VlAltura = table.Column<int>(nullable: false),
                    Qtpeso = table.Column<int>(nullable: false),
                    StatusAgendamento = table.Column<string>(nullable: false),
                    SNEncaixe = table.Column<bool>(nullable: true),
                    AtendimentoId = table.Column<int>(nullable: true),
                    UsuarioId = table.Column<string>(nullable: false),
                    ConvenioId = table.Column<int>(nullable: false),
                    ItemAgendamentoId = table.Column<int>(nullable: false),
                    Bloqueado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendamentos", x => x.AgendamentoId);
                    table.ForeignKey(
                        name: "FK_Agendamentos_AgendasMedicas_AgendaMedicaId",
                        column: x => x.AgendaMedicaId,
                        principalTable: "AgendasMedicas",
                        principalColumn: "AgendaMedicaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agendamentos_ItemAgendamentos_ItemAgendamentoId",
                        column: x => x.ItemAgendamentoId,
                        principalTable: "ItemAgendamentos",
                        principalColumn: "ItemAgendamentoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agendamentos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConveniosAgendasMedica",
                columns: table => new
                {
                    ConvenioId = table.Column<int>(nullable: false),
                    AgendaMedicaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConveniosAgendasMedica", x => new { x.ConvenioId, x.AgendaMedicaId });
                    table.ForeignKey(
                        name: "FK_ConveniosAgendasMedica_AgendasMedicas_AgendaMedicaId",
                        column: x => x.AgendaMedicaId,
                        principalTable: "AgendasMedicas",
                        principalColumn: "AgendaMedicaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Convenios",
                columns: table => new
                {
                    ConvenioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(maxLength: 160, nullable: false),
                    RazaoSocial = table.Column<string>(maxLength: 160, nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    CNPJ = table.Column<string>(maxLength: 160, nullable: false),
                    InscricaoMunicipal = table.Column<string>(nullable: true),
                    InscricaoEstadual = table.Column<string>(nullable: true),
                    RegistroAns = table.Column<string>(maxLength: 160, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    DtCadastro = table.Column<DateTime>(nullable: false),
                    DtAlteracao = table.Column<DateTime>(nullable: false),
                    CepId = table.Column<int>(nullable: true),
                    Numero = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    CartaoConvenioId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Convenios", x => x.ConvenioId);
                    table.ForeignKey(
                        name: "FK_Convenios_Cep_CepId",
                        column: x => x.CepId,
                        principalTable: "Cep",
                        principalColumn: "CepId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TabelaFaturamentos",
                columns: table => new
                {
                    TabelaFaturamentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: false),
                    ConvenioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabelaFaturamentos", x => x.TabelaFaturamentoId);
                    table.ForeignKey(
                        name: "FK_TabelaFaturamentos_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "ConvenioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TabelaFatuProcedimentos",
                columns: table => new
                {
                    TabelaFatuProcedimentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataVigencia = table.Column<DateTime>(type: "date", nullable: false),
                    ProcedimentoId = table.Column<int>(nullable: false),
                    ValorTotal = table.Column<decimal>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    TabelaFaturamentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabelaFatuProcedimentos", x => x.TabelaFatuProcedimentoId);
                    table.ForeignKey(
                        name: "FK_TabelaFatuProcedimentos_Procedimentos_ProcedimentoId",
                        column: x => x.ProcedimentoId,
                        principalTable: "Procedimentos",
                        principalColumn: "ProcedimentoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TabelaFatuProcedimentos_TabelaFaturamentos_TabelaFaturamentoId",
                        column: x => x.TabelaFaturamentoId,
                        principalTable: "TabelaFaturamentos",
                        principalColumn: "TabelaFaturamentoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    PacienteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(maxLength: 160, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "date", nullable: false),
                    CPF = table.Column<string>(maxLength: 16, nullable: true),
                    SemCPF = table.Column<bool>(nullable: false),
                    Sexo = table.Column<string>(maxLength: 60, nullable: true),
                    Estado_Civil = table.Column<string>(maxLength: 60, nullable: true),
                    Email = table.Column<string>(maxLength: 60, nullable: true),
                    RG = table.Column<string>(maxLength: 14, nullable: true),
                    Telefone = table.Column<string>(maxLength: 40, nullable: false),
                    dt_cadastro = table.Column<DateTime>(nullable: false),
                    StatusId = table.Column<bool>(nullable: false),
                    CepId = table.Column<int>(nullable: true),
                    Numero = table.Column<string>(maxLength: 5, nullable: true),
                    CartaoConvenioId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.PacienteId);
                    table.ForeignKey(
                        name: "FK_Pacientes_Cep_CepId",
                        column: x => x.CepId,
                        principalTable: "Cep",
                        principalColumn: "CepId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartaoConvenios",
                columns: table => new
                {
                    CartaoConvenioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConvenioId = table.Column<int>(nullable: false),
                    PacienteId = table.Column<int>(nullable: false),
                    NumeroCarteira = table.Column<string>(maxLength: 30, nullable: false),
                    Validade = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartaoConvenios", x => x.CartaoConvenioId);
                    table.ForeignKey(
                        name: "FK_CartaoConvenios_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "ConvenioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartaoConvenios_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "PacienteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    ContatoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(maxLength: 160, nullable: true),
                    Parentesco = table.Column<string>(maxLength: 160, nullable: true),
                    Email = table.Column<string>(maxLength: 160, nullable: true),
                    Telefone1 = table.Column<string>(maxLength: 160, nullable: true),
                    Telefone2 = table.Column<string>(maxLength: 160, nullable: true),
                    PacienteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contatos", x => x.ContatoId);
                    table.ForeignKey(
                        name: "FK_Contatos_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "PacienteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_AgendaMedicaId",
                table: "Agendamentos",
                column: "AgendaMedicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_ConvenioId",
                table: "Agendamentos",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_ItemAgendamentoId",
                table: "Agendamentos",
                column: "ItemAgendamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_PacienteId",
                table: "Agendamentos",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_UsuarioId",
                table: "Agendamentos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_AgendasMedicas_EmpresaId",
                table: "AgendasMedicas",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_AgendasMedicas_PrestadorId",
                table: "AgendasMedicas",
                column: "PrestadorId");

            migrationBuilder.CreateIndex(
                name: "IX_AgendasMedicas_RecursoAgendamentoId",
                table: "AgendasMedicas",
                column: "RecursoAgendamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_AgendasMedicas_SetorId",
                table: "AgendasMedicas",
                column: "SetorId");

            migrationBuilder.CreateIndex(
                name: "IX_AgendasMedicas_UsuarioId",
                table: "AgendasMedicas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CartaoConvenios_ConvenioId",
                table: "CartaoConvenios",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_CartaoConvenios_PacienteId",
                table: "CartaoConvenios",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_PacienteId",
                table: "Contatos",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Convenios_CNPJ",
                table: "Convenios",
                column: "CNPJ");

            migrationBuilder.CreateIndex(
                name: "IX_Convenios_CartaoConvenioId",
                table: "Convenios",
                column: "CartaoConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_Convenios_CepId",
                table: "Convenios",
                column: "CepId");

            migrationBuilder.CreateIndex(
                name: "IX_ConveniosAgendasMedica_AgendaMedicaId",
                table: "ConveniosAgendasMedica",
                column: "AgendaMedicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_CNPJ",
                table: "Empresas",
                column: "CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_CepId",
                table: "Empresas",
                column: "CepId");

            migrationBuilder.CreateIndex(
                name: "IX_Exames_EspecialidadeId",
                table: "Exames",
                column: "EspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Exames_SetorId",
                table: "Exames",
                column: "SetorId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAgendamentoPrestadores_ItemAgendamentoId",
                table: "ItemAgendamentoPrestadores",
                column: "ItemAgendamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAgendamentos_ExameId",
                table: "ItemAgendamentos",
                column: "ExameId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAgendamentos_RecursoAgendamentoId",
                table: "ItemAgendamentos",
                column: "RecursoAgendamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensAgendasMedica_AgendaMedicaId",
                table: "ItensAgendasMedica",
                column: "AgendaMedicaId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "NiveisAcessos",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_CPF",
                table: "Pacientes",
                column: "CPF",
                unique: true,
                filter: "[CPF] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_CartaoConvenioId",
                table: "Pacientes",
                column: "CartaoConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_CepId",
                table: "Pacientes",
                column: "CepId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestadores_CPF",
                table: "Prestadores",
                column: "CPF");

            migrationBuilder.CreateIndex(
                name: "IX_Prestadores_CepId",
                table: "Prestadores",
                column: "CepId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestadores_ConselhoId",
                table: "Prestadores",
                column: "ConselhoId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestadores_NumeroCrm",
                table: "Prestadores",
                column: "NumeroCrm");

            migrationBuilder.CreateIndex(
                name: "IX_Prestadores_TipoPrestadorId",
                table: "Prestadores",
                column: "TipoPrestadorId");

            migrationBuilder.CreateIndex(
                name: "IX_PrestadoresEspecialidades_EspecialidadeId",
                table: "PrestadoresEspecialidades",
                column: "EspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Procedimentos_GrupoFaturamentoId",
                table: "Procedimentos",
                column: "GrupoFaturamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_SobreUsuarios_CepId",
                table: "SobreUsuarios",
                column: "CepId");

            migrationBuilder.CreateIndex(
                name: "IX_SobreUsuarios_UsuarioId",
                table: "SobreUsuarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TabelaFatuProcedimentos_ProcedimentoId",
                table: "TabelaFatuProcedimentos",
                column: "ProcedimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_TabelaFatuProcedimentos_TabelaFaturamentoId",
                table: "TabelaFatuProcedimentos",
                column: "TabelaFaturamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_TabelaFaturamentos_ConvenioId",
                table: "TabelaFaturamentos",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CPF",
                table: "Usuarios",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CepId",
                table: "Usuarios",
                column: "CepId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Usuarios",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Usuarios",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_SetorId",
                table: "Usuarios",
                column: "SetorId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosEmpresas_EmpresaId",
                table: "UsuariosEmpresas",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Convenios_ConvenioId",
                table: "Agendamentos",
                column: "ConvenioId",
                principalTable: "Convenios",
                principalColumn: "ConvenioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Pacientes_PacienteId",
                table: "Agendamentos",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "PacienteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConveniosAgendasMedica_Convenios_ConvenioId",
                table: "ConveniosAgendasMedica",
                column: "ConvenioId",
                principalTable: "Convenios",
                principalColumn: "ConvenioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Convenios_CartaoConvenios_CartaoConvenioId",
                table: "Convenios",
                column: "CartaoConvenioId",
                principalTable: "CartaoConvenios",
                principalColumn: "CartaoConvenioId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_CartaoConvenios_CartaoConvenioId",
                table: "Pacientes",
                column: "CartaoConvenioId",
                principalTable: "CartaoConvenios",
                principalColumn: "CartaoConvenioId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartaoConvenios_Convenios_ConvenioId",
                table: "CartaoConvenios");

            migrationBuilder.DropForeignKey(
                name: "FK_CartaoConvenios_Pacientes_PacienteId",
                table: "CartaoConvenios");

            migrationBuilder.DropTable(
                name: "Agendamentos");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Contatos");

            migrationBuilder.DropTable(
                name: "ConveniosAgendasMedica");

            migrationBuilder.DropTable(
                name: "ItemAgendamentoPrestadores");

            migrationBuilder.DropTable(
                name: "ItensAgendasMedica");

            migrationBuilder.DropTable(
                name: "PrestadoresEspecialidades");

            migrationBuilder.DropTable(
                name: "SobreUsuarios");

            migrationBuilder.DropTable(
                name: "TabelaFatuProcedimentos");

            migrationBuilder.DropTable(
                name: "UsuariosEmpresas");

            migrationBuilder.DropTable(
                name: "NiveisAcessos");

            migrationBuilder.DropTable(
                name: "AgendasMedicas");

            migrationBuilder.DropTable(
                name: "ItemAgendamentos");

            migrationBuilder.DropTable(
                name: "Procedimentos");

            migrationBuilder.DropTable(
                name: "TabelaFaturamentos");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Prestadores");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Exames");

            migrationBuilder.DropTable(
                name: "RecursoAgendamentos");

            migrationBuilder.DropTable(
                name: "GrupoFaturamentos");

            migrationBuilder.DropTable(
                name: "Conselhos");

            migrationBuilder.DropTable(
                name: "TipoPrestadores");

            migrationBuilder.DropTable(
                name: "Especialidades");

            migrationBuilder.DropTable(
                name: "Setores");

            migrationBuilder.DropTable(
                name: "Convenios");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "CartaoConvenios");

            migrationBuilder.DropTable(
                name: "Cep");
        }
    }
}
