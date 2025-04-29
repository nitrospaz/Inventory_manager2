using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventory_manager.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliverableItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliverableItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Project = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Categories = table.Column<string>(type: "TEXT", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliverableItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeliverableItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    AuthorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => new { x.DeliverableItemId, x.Timestamp });
                    table.ForeignKey(
                        name: "FK_Comments_DeliverableItems_DeliverableItemId",
                        column: x => x.DeliverableItemId,
                        principalTable: "DeliverableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_People_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliverableItemAssignedTo",
                columns: table => new
                {
                    AssignedToId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeliverableItemId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliverableItemAssignedTo", x => new { x.AssignedToId, x.DeliverableItemId });
                    table.ForeignKey(
                        name: "FK_DeliverableItemAssignedTo_DeliverableItems_DeliverableItemId",
                        column: x => x.DeliverableItemId,
                        principalTable: "DeliverableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliverableItemAssignedTo_People_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliverableItemNotifyList",
                columns: table => new
                {
                    DeliverableItem1Id = table.Column<int>(type: "INTEGER", nullable: false),
                    NotifyListId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliverableItemNotifyList", x => new { x.DeliverableItem1Id, x.NotifyListId });
                    table.ForeignKey(
                        name: "FK_DeliverableItemNotifyList_DeliverableItems_DeliverableItem1Id",
                        column: x => x.DeliverableItem1Id,
                        principalTable: "DeliverableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliverableItemNotifyList_People_NotifyListId",
                        column: x => x.NotifyListId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliverableItemAssignedTo_DeliverableItemId",
                table: "DeliverableItemAssignedTo",
                column: "DeliverableItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliverableItemNotifyList_NotifyListId",
                table: "DeliverableItemNotifyList",
                column: "NotifyListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "DeliverableItemAssignedTo");

            migrationBuilder.DropTable(
                name: "DeliverableItemNotifyList");

            migrationBuilder.DropTable(
                name: "DeliverableItems");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
