using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AutoPendingChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_documents_document_statuses_document_current_status_id",
                table: "documents");

            migrationBuilder.DropForeignKey(
                name: "fk_documents_document_statuses_document_dean_status_id",
                table: "documents");

            migrationBuilder.DropForeignKey(
                name: "fk_documents_document_statuses_document_secretary_status_id",
                table: "documents");

            migrationBuilder.DropForeignKey(
                name: "fk_documents_users_resolved_by_user_id",
                table: "documents");

            migrationBuilder.DropForeignKey(
                name: "fk_staff_profiles_academic_groups_academic_group_id",
                table: "staff_profiles");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_subjects_name",
                table: "subjects",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_student_statuses_name",
                table: "student_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_specialties_code",
                table: "specialties",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_specialties_name",
                table: "specialties",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_roles_system_name",
                table: "roles",
                column: "system_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_document_types_name",
                table: "document_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_document_statuses_name",
                table: "document_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_departments_name",
                table: "departments",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_academic_groups_name",
                table: "academic_groups",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_documents_document_statuses_document_current_status_id",
                table: "documents",
                column: "document_current_status_id",
                principalTable: "document_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_documents_document_statuses_document_dean_status_id",
                table: "documents",
                column: "document_dean_status_id",
                principalTable: "document_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_documents_document_statuses_document_secretary_status_id",
                table: "documents",
                column: "document_secretary_status_id",
                principalTable: "document_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_documents_users_resolved_by_user_id",
                table: "documents",
                column: "resolved_by_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_staff_profiles_academic_groups_academic_group_id",
                table: "staff_profiles",
                column: "academic_group_id",
                principalTable: "academic_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_documents_document_statuses_document_current_status_id",
                table: "documents");

            migrationBuilder.DropForeignKey(
                name: "fk_documents_document_statuses_document_dean_status_id",
                table: "documents");

            migrationBuilder.DropForeignKey(
                name: "fk_documents_document_statuses_document_secretary_status_id",
                table: "documents");

            migrationBuilder.DropForeignKey(
                name: "fk_documents_users_resolved_by_user_id",
                table: "documents");

            migrationBuilder.DropForeignKey(
                name: "fk_staff_profiles_academic_groups_academic_group_id",
                table: "staff_profiles");

            migrationBuilder.DropIndex(
                name: "ix_users_email",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_subjects_name",
                table: "subjects");

            migrationBuilder.DropIndex(
                name: "ix_student_statuses_name",
                table: "student_statuses");

            migrationBuilder.DropIndex(
                name: "ix_specialties_code",
                table: "specialties");

            migrationBuilder.DropIndex(
                name: "ix_specialties_name",
                table: "specialties");

            migrationBuilder.DropIndex(
                name: "ix_roles_system_name",
                table: "roles");

            migrationBuilder.DropIndex(
                name: "ix_document_types_name",
                table: "document_types");

            migrationBuilder.DropIndex(
                name: "ix_document_statuses_name",
                table: "document_statuses");

            migrationBuilder.DropIndex(
                name: "ix_departments_name",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "ix_academic_groups_name",
                table: "academic_groups");

            migrationBuilder.AddForeignKey(
                name: "fk_documents_document_statuses_document_current_status_id",
                table: "documents",
                column: "document_current_status_id",
                principalTable: "document_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_documents_document_statuses_document_dean_status_id",
                table: "documents",
                column: "document_dean_status_id",
                principalTable: "document_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_documents_document_statuses_document_secretary_status_id",
                table: "documents",
                column: "document_secretary_status_id",
                principalTable: "document_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_documents_users_resolved_by_user_id",
                table: "documents",
                column: "resolved_by_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_staff_profiles_academic_groups_academic_group_id",
                table: "staff_profiles",
                column: "academic_group_id",
                principalTable: "academic_groups",
                principalColumn: "id");
        }
    }
}
