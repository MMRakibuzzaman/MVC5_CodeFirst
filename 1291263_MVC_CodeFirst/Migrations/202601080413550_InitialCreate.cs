namespace _1291263_MVC_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jerseys",
                c => new
                    {
                        JerseyId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ReleaseDate = c.DateTime(nullable: false, storeType: "date"),
                        IsAvailable = c.Boolean(nullable: false),
                        Picture = c.String(),
                        TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JerseyId)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
            CreateTable(
                "dbo.JerseyStocks",
                c => new
                    {
                        JerseyStockId = c.Int(nullable: false, identity: true),
                        Size = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        JerseyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JerseyStockId)
                .ForeignKey("dbo.Jerseys", t => t.JerseyId, cascadeDelete: true)
                .Index(t => t.JerseyId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        TeamName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.TeamId);
            
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
            
            CreateStoredProcedure(
                "dbo.Jersey_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 100),
                        ReleaseDate = p.DateTime(storeType: "date"),
                        IsAvailable = p.Boolean(),
                        Picture = p.String(),
                        TeamId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Jerseys]([Name], [ReleaseDate], [IsAvailable], [Picture], [TeamId])
                      VALUES (@Name, @ReleaseDate, @IsAvailable, @Picture, @TeamId)
                      
                      DECLARE @JerseyId int
                      SELECT @JerseyId = [JerseyId]
                      FROM [dbo].[Jerseys]
                      WHERE @@ROWCOUNT > 0 AND [JerseyId] = scope_identity()
                      
                      SELECT t0.[JerseyId]
                      FROM [dbo].[Jerseys] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[JerseyId] = @JerseyId"
            );
            
            CreateStoredProcedure(
                "dbo.Jersey_Update",
                p => new
                    {
                        JerseyId = p.Int(),
                        Name = p.String(maxLength: 100),
                        ReleaseDate = p.DateTime(storeType: "date"),
                        IsAvailable = p.Boolean(),
                        Picture = p.String(),
                        TeamId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Jerseys]
                      SET [Name] = @Name, [ReleaseDate] = @ReleaseDate, [IsAvailable] = @IsAvailable, [Picture] = @Picture, [TeamId] = @TeamId
                      WHERE ([JerseyId] = @JerseyId)"
            );
            
            CreateStoredProcedure(
                "dbo.Jersey_Delete",
                p => new
                    {
                        JerseyId = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Jerseys]
                      WHERE ([JerseyId] = @JerseyId)"
            );
            
            CreateStoredProcedure(
                "dbo.JerseyStock_Insert",
                p => new
                    {
                        Size = p.Int(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        Quantity = p.Int(),
                        JerseyId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[JerseyStocks]([Size], [Price], [Quantity], [JerseyId])
                      VALUES (@Size, @Price, @Quantity, @JerseyId)
                      
                      DECLARE @JerseyStockId int
                      SELECT @JerseyStockId = [JerseyStockId]
                      FROM [dbo].[JerseyStocks]
                      WHERE @@ROWCOUNT > 0 AND [JerseyStockId] = scope_identity()
                      
                      SELECT t0.[JerseyStockId]
                      FROM [dbo].[JerseyStocks] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[JerseyStockId] = @JerseyStockId"
            );
            
            CreateStoredProcedure(
                "dbo.JerseyStock_Update",
                p => new
                    {
                        JerseyStockId = p.Int(),
                        Size = p.Int(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        Quantity = p.Int(),
                        JerseyId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[JerseyStocks]
                      SET [Size] = @Size, [Price] = @Price, [Quantity] = @Quantity, [JerseyId] = @JerseyId
                      WHERE ([JerseyStockId] = @JerseyStockId)"
            );
            
            CreateStoredProcedure(
                "dbo.JerseyStock_Delete",
                p => new
                    {
                        JerseyStockId = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JerseyStocks]
                      WHERE ([JerseyStockId] = @JerseyStockId)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.JerseyStock_Delete");
            DropStoredProcedure("dbo.JerseyStock_Update");
            DropStoredProcedure("dbo.JerseyStock_Insert");
            DropStoredProcedure("dbo.Jersey_Delete");
            DropStoredProcedure("dbo.Jersey_Update");
            DropStoredProcedure("dbo.Jersey_Insert");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Jerseys", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.JerseyStocks", "JerseyId", "dbo.Jerseys");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.JerseyStocks", new[] { "JerseyId" });
            DropIndex("dbo.Jerseys", new[] { "TeamId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Teams");
            DropTable("dbo.JerseyStocks");
            DropTable("dbo.Jerseys");
        }
    }
}
