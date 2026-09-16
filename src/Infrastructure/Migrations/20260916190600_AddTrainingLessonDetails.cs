using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingCatalog.Infrastructure.Migrations;

public partial class AddTrainingLessonDetails : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "LessonCount",
            table: "Trainings",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "LessonDurationHours",
            table: "Trainings",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.Sql("""
            UPDATE Trainings
            SET LessonCount = 1,
                LessonDurationHours = CASE
                    WHEN DurationHours > 4 THEN 4
                    ELSE DurationHours
                END;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "LessonCount",
            table: "Trainings");

        migrationBuilder.DropColumn(
            name: "LessonDurationHours",
            table: "Trainings");
    }
}
