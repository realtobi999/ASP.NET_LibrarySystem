using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Migration_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Books_book_id",
                table: "BookGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Genres_genre_id",
                table: "BookGenre");

            migrationBuilder.DropTable(
                name: "BookAuthor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "cover_photo",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "profile_photo",
                table: "Authors");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.RenameColumn(
                name: "genre_id",
                table: "BookGenre",
                newName: "GenresId");

            migrationBuilder.RenameColumn(
                name: "book_id",
                table: "BookGenre",
                newName: "BooksId");

            migrationBuilder.RenameIndex(
                name: "IX_BookGenre_genre_id",
                table: "BookGenre",
                newName: "IX_BookGenre_GenresId");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_email",
                table: "Employees",
                newName: "IX_Employees_email");

            migrationBuilder.AddColumn<Guid>(
                name: "WishlistId",
                table: "Books",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "available",
                table: "Books",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "Books",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<double>(
                name: "popularity",
                table: "Books",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "birthday",
                table: "Authors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "id");

            migrationBuilder.CreateTable(
                name: "AuthorBook",
                columns: table => new
                {
                    AuthorsId = table.Column<Guid>(type: "uuid", nullable: false),
                    BooksId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorsId, x.BooksId });
                    table.ForeignKey(
                        name: "FK_AuthorBook_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBook_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookReviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<double>(type: "double precision", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookReviews", x => x.id);
                    table.ForeignKey(
                        name: "FK_BookReviews_Books_book_id",
                        column: x => x.book_id,
                        principalTable: "Books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookReviews_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Borrows",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    borrow_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    borrow_due = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsReturned = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrows", x => x.id);
                    table.ForeignKey(
                        name: "FK_Borrows_Books_book_id",
                        column: x => x.book_id,
                        principalTable: "Books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Borrows_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_type = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<byte[]>(type: "bytea", nullable: false),
                    mime_type = table.Column<string>(type: "text", nullable: false),
                    entity_type1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.id);
                    table.ForeignKey(
                        name: "FK_Pictures_Authors_book_id",
                        column: x => x.book_id,
                        principalTable: "Authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pictures_Books_book_id",
                        column: x => x.book_id,
                        principalTable: "Books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pictures_Employees_book_id",
                        column: x => x.book_id,
                        principalTable: "Employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pictures_Users_book_id",
                        column: x => x.book_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.id);
                    table.ForeignKey(
                        name: "FK_Wishlists_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_WishlistId",
                table: "Books",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_BooksId",
                table: "AuthorBook",
                column: "BooksId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReviews_book_id",
                table: "BookReviews",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookReviews_user_id",
                table: "BookReviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_book_id",
                table: "Borrows",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_user_id",
                table: "Borrows",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_book_id",
                table: "Pictures",
                column: "book_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_user_id",
                table: "Wishlists",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Books_BooksId",
                table: "BookGenre",
                column: "BooksId",
                principalTable: "Books",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Genres_GenresId",
                table: "BookGenre",
                column: "GenresId",
                principalTable: "Genres",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Wishlists_WishlistId",
                table: "Books",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Books_BooksId",
                table: "BookGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Genres_GenresId",
                table: "BookGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Wishlists_WishlistId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "AuthorBook");

            migrationBuilder.DropTable(
                name: "BookReviews");

            migrationBuilder.DropTable(
                name: "Borrows");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Books_WishlistId",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WishlistId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "available",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "popularity",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "birthday",
                table: "Authors");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.RenameColumn(
                name: "GenresId",
                table: "BookGenre",
                newName: "genre_id");

            migrationBuilder.RenameColumn(
                name: "BooksId",
                table: "BookGenre",
                newName: "book_id");

            migrationBuilder.RenameIndex(
                name: "IX_BookGenre_GenresId",
                table: "BookGenre",
                newName: "IX_BookGenre_genre_id");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_email",
                table: "Employee",
                newName: "IX_Employee_email");

            migrationBuilder.AddColumn<byte[]>(
                name: "cover_photo",
                table: "Books",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "profile_photo",
                table: "Authors",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "id");

            migrationBuilder.CreateTable(
                name: "BookAuthor",
                columns: table => new
                {
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthor", x => new { x.book_id, x.author_id });
                    table.ForeignKey(
                        name: "FK_BookAuthor_Authors_author_id",
                        column: x => x.author_id,
                        principalTable: "Authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthor_Books_book_id",
                        column: x => x.book_id,
                        principalTable: "Books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthor_author_id",
                table: "BookAuthor",
                column: "author_id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Books_book_id",
                table: "BookGenre",
                column: "book_id",
                principalTable: "Books",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Genres_genre_id",
                table: "BookGenre",
                column: "genre_id",
                principalTable: "Genres",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
