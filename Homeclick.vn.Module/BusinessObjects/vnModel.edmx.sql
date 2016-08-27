
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/23/2016 18:06:58
-- Generated from EDMX file: G:\VS2010\Projects\Vinabits\homeclick\Homeclick.vn\Homeclick.vn.Module\BusinessObjects\vnModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [vinabits_homeclick];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Users_Roles_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users_Roles] DROP CONSTRAINT [FK_Users_Roles_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Users_Roles_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users_Roles] DROP CONSTRAINT [FK_Users_Roles_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_Role_TypePermissionObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TypePermissionObjects] DROP CONSTRAINT [FK_Role_TypePermissionObject];
GO
IF OBJECT_ID(N'[dbo].[FK_Roles_Roles_Role1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Roles_Roles] DROP CONSTRAINT [FK_Roles_Roles_Role1];
GO
IF OBJECT_ID(N'[dbo].[FK_Roles_Roles_Role2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Roles_Roles] DROP CONSTRAINT [FK_Roles_Roles_Role2];
GO
IF OBJECT_ID(N'[dbo].[FK_TypePermissionObjectMemberPermissionsObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SecuritySystemMemberPermissionsObjects] DROP CONSTRAINT [FK_TypePermissionObjectMemberPermissionsObject];
GO
IF OBJECT_ID(N'[dbo].[FK_TypePermissionObjectObjectPermissionsObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SecuritySystemObjectPermissionsObjects] DROP CONSTRAINT [FK_TypePermissionObjectObjectPermissionsObject];
GO
IF OBJECT_ID(N'[dbo].[FK_ModelDifferenceAspect_ModelDifference]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ModelDifferenceAspects] DROP CONSTRAINT [FK_ModelDifferenceAspect_ModelDifference];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductsProduct_types]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductSet] DROP CONSTRAINT [FK_ProductsProduct_types];
GO
IF OBJECT_ID(N'[dbo].[FK_Product_detailsProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Product_detailSet] DROP CONSTRAINT [FK_Product_detailsProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoriesCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CategorySet] DROP CONSTRAINT [FK_CategoriesCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoriesCategory_types]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CategorySet] DROP CONSTRAINT [FK_CategoriesCategory_types];
GO
IF OBJECT_ID(N'[dbo].[FK_Category_detailsCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Category_detailSet] DROP CONSTRAINT [FK_Category_detailsCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductsCategories_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductsCategories] DROP CONSTRAINT [FK_ProductsCategories_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductsCategories_Categories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductsCategories] DROP CONSTRAINT [FK_ProductsCategories_Categories];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductsTags_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductsTags] DROP CONSTRAINT [FK_ProductsTags_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductsTags_Tags]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductsTags] DROP CONSTRAINT [FK_ProductsTags_Tags];
GO
IF OBJECT_ID(N'[dbo].[FK_Post_detailsPosts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Post_detailSet] DROP CONSTRAINT [FK_Post_detailsPosts];
GO
IF OBJECT_ID(N'[dbo].[FK_PostsCategories_Posts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostsCategories] DROP CONSTRAINT [FK_PostsCategories_Posts];
GO
IF OBJECT_ID(N'[dbo].[FK_PostsCategories_Categories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostsCategories] DROP CONSTRAINT [FK_PostsCategories_Categories];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentsDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentSet] DROP CONSTRAINT [FK_DepartmentsDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_FloorsDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FloorSet] DROP CONSTRAINT [FK_FloorsDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_RoomsFloors]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoomSet] DROP CONSTRAINT [FK_RoomsFloors];
GO
IF OBJECT_ID(N'[dbo].[FK_CanvasRooms]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CanvaSet] DROP CONSTRAINT [FK_CanvasRooms];
GO
IF OBJECT_ID(N'[dbo].[FK_CartsCanvas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CanvaSet] DROP CONSTRAINT [FK_CartsCanvas];
GO
IF OBJECT_ID(N'[dbo].[FK_CartsProducts_Carts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CartsProducts] DROP CONSTRAINT [FK_CartsProducts_Carts];
GO
IF OBJECT_ID(N'[dbo].[FK_CartsProducts_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CartsProducts] DROP CONSTRAINT [FK_CartsProducts_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_UserCarts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CartSet] DROP CONSTRAINT [FK_UserCarts];
GO
IF OBJECT_ID(N'[dbo].[FK_UserCanvas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CanvaSet] DROP CONSTRAINT [FK_UserCanvas];
GO
IF OBJECT_ID(N'[dbo].[FK_UserProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductSet] DROP CONSTRAINT [FK_UserProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_UserPosts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostSet] DROP CONSTRAINT [FK_UserPosts];
GO
IF OBJECT_ID(N'[dbo].[FK_UserWishlists]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WishlistSet] DROP CONSTRAINT [FK_UserWishlists];
GO
IF OBJECT_ID(N'[dbo].[FK_WishlistsProducts_Wishlists]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WishlistsProducts] DROP CONSTRAINT [FK_WishlistsProducts_Wishlists];
GO
IF OBJECT_ID(N'[dbo].[FK_WishlistsProducts_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WishlistsProducts] DROP CONSTRAINT [FK_WishlistsProducts_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_PostPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostSet] DROP CONSTRAINT [FK_PostPost];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerSet] DROP CONSTRAINT [FK_CustomerUser];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCustomer_type]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerSet] DROP CONSTRAINT [FK_CustomerCustomer_type];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FileData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FileData];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[TypePermissionObjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TypePermissionObjects];
GO
IF OBJECT_ID(N'[dbo].[SecuritySystemObjectPermissionsObjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SecuritySystemObjectPermissionsObjects];
GO
IF OBJECT_ID(N'[dbo].[SecuritySystemMemberPermissionsObjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SecuritySystemMemberPermissionsObjects];
GO
IF OBJECT_ID(N'[dbo].[ModulesInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ModulesInfo];
GO
IF OBJECT_ID(N'[dbo].[ModelDifferences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ModelDifferences];
GO
IF OBJECT_ID(N'[dbo].[ModelDifferenceAspects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ModelDifferenceAspects];
GO
IF OBJECT_ID(N'[dbo].[ProductSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductSet];
GO
IF OBJECT_ID(N'[dbo].[Product_typeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Product_typeSet];
GO
IF OBJECT_ID(N'[dbo].[Product_detailSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Product_detailSet];
GO
IF OBJECT_ID(N'[dbo].[CategorySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CategorySet];
GO
IF OBJECT_ID(N'[dbo].[Category_typeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category_typeSet];
GO
IF OBJECT_ID(N'[dbo].[Category_detailSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category_detailSet];
GO
IF OBJECT_ID(N'[dbo].[TagSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TagSet];
GO
IF OBJECT_ID(N'[dbo].[PostSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostSet];
GO
IF OBJECT_ID(N'[dbo].[Post_detailSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Post_detailSet];
GO
IF OBJECT_ID(N'[dbo].[DepartmentSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentSet];
GO
IF OBJECT_ID(N'[dbo].[FloorSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FloorSet];
GO
IF OBJECT_ID(N'[dbo].[RoomSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoomSet];
GO
IF OBJECT_ID(N'[dbo].[CanvaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CanvaSet];
GO
IF OBJECT_ID(N'[dbo].[CartSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CartSet];
GO
IF OBJECT_ID(N'[dbo].[WishlistSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WishlistSet];
GO
IF OBJECT_ID(N'[dbo].[CustomerSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerSet];
GO
IF OBJECT_ID(N'[dbo].[Customer_typeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customer_typeSet];
GO
IF OBJECT_ID(N'[dbo].[Users_Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users_Roles];
GO
IF OBJECT_ID(N'[dbo].[Roles_Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles_Roles];
GO
IF OBJECT_ID(N'[dbo].[ProductsCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductsCategories];
GO
IF OBJECT_ID(N'[dbo].[ProductsTags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductsTags];
GO
IF OBJECT_ID(N'[dbo].[PostsCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostsCategories];
GO
IF OBJECT_ID(N'[dbo].[CartsProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CartsProducts];
GO
IF OBJECT_ID(N'[dbo].[WishlistsProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WishlistsProducts];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'FileData'
CREATE TABLE [dbo].[FileData] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Size] int  NOT NULL,
    [FileName] nvarchar(max)  NULL,
    [Content] varbinary(max)  NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsAdministrative] bit  NOT NULL,
    [CanEditModel] bit  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [ChangePasswordOnFirstLogon] bit  NOT NULL,
    [StoredPassword] nvarchar(max)  NULL
);
GO

-- Creating table 'TypePermissionObjects'
CREATE TABLE [dbo].[TypePermissionObjects] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AllowRead] bit  NOT NULL,
    [AllowWrite] bit  NOT NULL,
    [AllowCreate] bit  NOT NULL,
    [AllowDelete] bit  NOT NULL,
    [AllowNavigate] bit  NOT NULL,
    [TargetTypeFullName] nvarchar(max)  NOT NULL,
    [Role_ID] int  NULL
);
GO

-- Creating table 'SecuritySystemObjectPermissionsObjects'
CREATE TABLE [dbo].[SecuritySystemObjectPermissionsObjects] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Criteria] nvarchar(max)  NULL,
    [AllowRead] bit  NOT NULL,
    [AllowWrite] bit  NOT NULL,
    [AllowDelete] bit  NOT NULL,
    [AllowNavigate] bit  NOT NULL,
    [Owner_ID] int  NOT NULL
);
GO

-- Creating table 'SecuritySystemMemberPermissionsObjects'
CREATE TABLE [dbo].[SecuritySystemMemberPermissionsObjects] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Members] nvarchar(max)  NOT NULL,
    [AllowRead] bit  NOT NULL,
    [AllowWrite] bit  NOT NULL,
    [Criteria] nvarchar(max)  NULL,
    [Owner_ID] int  NOT NULL
);
GO

-- Creating table 'ModulesInfo'
CREATE TABLE [dbo].[ModulesInfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Version] nvarchar(max)  NULL,
    [AssemblyFileName] nvarchar(max)  NULL,
    [IsMain] bit  NOT NULL
);
GO

-- Creating table 'ModelDifferences'
CREATE TABLE [dbo].[ModelDifferences] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(max)  NULL,
    [ContextId] nvarchar(max)  NULL,
    [Version] int  NOT NULL
);
GO

-- Creating table 'ModelDifferenceAspects'
CREATE TABLE [dbo].[ModelDifferenceAspects] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Xml] nvarchar(max)  NULL,
    [Owner_ID] int  NOT NULL
);
GO

-- Creating table 'ProductSet'
CREATE TABLE [dbo].[ProductSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [content] nvarchar(max)  NULL,
    [created_date] datetime  NOT NULL,
    [updated_date] datetime  NOT NULL,
    [status] smallint  NOT NULL,
    [Product_type_id] int  NOT NULL,
    [author_id] int  NOT NULL
);
GO

-- Creating table 'Product_typeSet'
CREATE TABLE [dbo].[Product_typeSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [caption] nvarchar(max)  NULL
);
GO

-- Creating table 'Product_detailSet'
CREATE TABLE [dbo].[Product_detailSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductsId] int  NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [value] nvarchar(max)  NULL,
    [caption] nvarchar(max)  NULL
);
GO

-- Creating table 'CategorySet'
CREATE TABLE [dbo].[CategorySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [description] nvarchar(max)  NULL,
    [parent_id] int  NULL,
    [Category_type_id] int  NOT NULL
);
GO

-- Creating table 'Category_typeSet'
CREATE TABLE [dbo].[Category_typeSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [caption] nvarchar(max)  NULL
);
GO

-- Creating table 'Category_detailSet'
CREATE TABLE [dbo].[Category_detailSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [value] nvarchar(max)  NULL,
    [caption] nvarchar(max)  NULL,
    [Category_Id] int  NOT NULL
);
GO

-- Creating table 'TagSet'
CREATE TABLE [dbo].[TagSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [description] nvarchar(max)  NULL
);
GO

-- Creating table 'PostSet'
CREATE TABLE [dbo].[PostSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [title] nvarchar(max)  NOT NULL,
    [alias] nvarchar(max)  NOT NULL,
    [content] nvarchar(max)  NULL,
    [author_id] int  NOT NULL,
    [created_date] datetime  NOT NULL,
    [updated_date] datetime  NOT NULL,
    [parent_id] int  NULL,
    [status] smallint  NOT NULL
);
GO

-- Creating table 'Post_detailSet'
CREATE TABLE [dbo].[Post_detailSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [post_id] int  NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [value] nvarchar(max)  NULL,
    [caption] nvarchar(max)  NULL
);
GO

-- Creating table 'DepartmentSet'
CREATE TABLE [dbo].[DepartmentSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [address] nvarchar(max)  NULL,
    [desciption] nvarchar(max)  NULL,
    [parent_id] int  NULL
);
GO

-- Creating table 'FloorSet'
CREATE TABLE [dbo].[FloorSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [description] nvarchar(max)  NULL,
    [block_id] int  NOT NULL,
    [structure_link] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RoomSet'
CREATE TABLE [dbo].[RoomSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [description] nvarchar(max)  NULL,
    [floor_id] int  NOT NULL,
    [coordinates] nvarchar(max)  NOT NULL,
    [canvas_data] nvarchar(max)  NULL
);
GO

-- Creating table 'CanvaSet'
CREATE TABLE [dbo].[CanvaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [room_id] int  NOT NULL,
    [user_id] int  NULL,
    [json_data] nvarchar(max)  NULL,
    [cart_id] int  NULL
);
GO

-- Creating table 'CartSet'
CREATE TABLE [dbo].[CartSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [user_id] int  NOT NULL,
    [created_date] datetime  NOT NULL,
    [status] smallint  NOT NULL
);
GO

-- Creating table 'WishlistSet'
CREATE TABLE [dbo].[WishlistSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [user_id] int  NOT NULL,
    [title] nvarchar(max)  NOT NULL,
    [created_date] nvarchar(max)  NOT NULL,
    [description] nvarchar(max)  NULL
);
GO

-- Creating table 'CustomerSet'
CREATE TABLE [dbo].[CustomerSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [first_name] nvarchar(max)  NOT NULL,
    [middle_name] nvarchar(max)  NOT NULL,
    [last_name] nvarchar(max)  NOT NULL,
    [address] nvarchar(max)  NOT NULL,
    [customer_type_id] int  NOT NULL,
    [phone] nvarchar(max)  NOT NULL,
    [User_ID] int  NOT NULL
);
GO

-- Creating table 'Customer_typeSet'
CREATE TABLE [dbo].[Customer_typeSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Users_Roles'
CREATE TABLE [dbo].[Users_Roles] (
    [Users_ID] int  NOT NULL,
    [Roles_ID] int  NOT NULL
);
GO

-- Creating table 'Roles_Roles'
CREATE TABLE [dbo].[Roles_Roles] (
    [ParentRoles_ID] int  NOT NULL,
    [ChildRoles_ID] int  NOT NULL
);
GO

-- Creating table 'ProductsCategories'
CREATE TABLE [dbo].[ProductsCategories] (
    [Products_Id] int  NOT NULL,
    [Categories_Id] int  NOT NULL
);
GO

-- Creating table 'ProductsTags'
CREATE TABLE [dbo].[ProductsTags] (
    [Products_Id] int  NOT NULL,
    [Tags_Id] int  NOT NULL
);
GO

-- Creating table 'PostsCategories'
CREATE TABLE [dbo].[PostsCategories] (
    [Posts_Id] int  NOT NULL,
    [Categories_Id] int  NOT NULL
);
GO

-- Creating table 'CartsProducts'
CREATE TABLE [dbo].[CartsProducts] (
    [Carts_Id] int  NOT NULL,
    [Products_Id] int  NOT NULL
);
GO

-- Creating table 'WishlistsProducts'
CREATE TABLE [dbo].[WishlistsProducts] (
    [Wishlists_Id] int  NOT NULL,
    [Products_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'FileData'
ALTER TABLE [dbo].[FileData]
ADD CONSTRAINT [PK_FileData]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TypePermissionObjects'
ALTER TABLE [dbo].[TypePermissionObjects]
ADD CONSTRAINT [PK_TypePermissionObjects]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SecuritySystemObjectPermissionsObjects'
ALTER TABLE [dbo].[SecuritySystemObjectPermissionsObjects]
ADD CONSTRAINT [PK_SecuritySystemObjectPermissionsObjects]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SecuritySystemMemberPermissionsObjects'
ALTER TABLE [dbo].[SecuritySystemMemberPermissionsObjects]
ADD CONSTRAINT [PK_SecuritySystemMemberPermissionsObjects]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ModulesInfo'
ALTER TABLE [dbo].[ModulesInfo]
ADD CONSTRAINT [PK_ModulesInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ModelDifferences'
ALTER TABLE [dbo].[ModelDifferences]
ADD CONSTRAINT [PK_ModelDifferences]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ModelDifferenceAspects'
ALTER TABLE [dbo].[ModelDifferenceAspects]
ADD CONSTRAINT [PK_ModelDifferenceAspects]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'ProductSet'
ALTER TABLE [dbo].[ProductSet]
ADD CONSTRAINT [PK_ProductSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Product_typeSet'
ALTER TABLE [dbo].[Product_typeSet]
ADD CONSTRAINT [PK_Product_typeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Product_detailSet'
ALTER TABLE [dbo].[Product_detailSet]
ADD CONSTRAINT [PK_Product_detailSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CategorySet'
ALTER TABLE [dbo].[CategorySet]
ADD CONSTRAINT [PK_CategorySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Category_typeSet'
ALTER TABLE [dbo].[Category_typeSet]
ADD CONSTRAINT [PK_Category_typeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Category_detailSet'
ALTER TABLE [dbo].[Category_detailSet]
ADD CONSTRAINT [PK_Category_detailSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TagSet'
ALTER TABLE [dbo].[TagSet]
ADD CONSTRAINT [PK_TagSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PostSet'
ALTER TABLE [dbo].[PostSet]
ADD CONSTRAINT [PK_PostSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Post_detailSet'
ALTER TABLE [dbo].[Post_detailSet]
ADD CONSTRAINT [PK_Post_detailSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DepartmentSet'
ALTER TABLE [dbo].[DepartmentSet]
ADD CONSTRAINT [PK_DepartmentSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FloorSet'
ALTER TABLE [dbo].[FloorSet]
ADD CONSTRAINT [PK_FloorSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RoomSet'
ALTER TABLE [dbo].[RoomSet]
ADD CONSTRAINT [PK_RoomSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CanvaSet'
ALTER TABLE [dbo].[CanvaSet]
ADD CONSTRAINT [PK_CanvaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CartSet'
ALTER TABLE [dbo].[CartSet]
ADD CONSTRAINT [PK_CartSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WishlistSet'
ALTER TABLE [dbo].[WishlistSet]
ADD CONSTRAINT [PK_WishlistSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustomerSet'
ALTER TABLE [dbo].[CustomerSet]
ADD CONSTRAINT [PK_CustomerSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Customer_typeSet'
ALTER TABLE [dbo].[Customer_typeSet]
ADD CONSTRAINT [PK_Customer_typeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Users_ID], [Roles_ID] in table 'Users_Roles'
ALTER TABLE [dbo].[Users_Roles]
ADD CONSTRAINT [PK_Users_Roles]
    PRIMARY KEY CLUSTERED ([Users_ID], [Roles_ID] ASC);
GO

-- Creating primary key on [ParentRoles_ID], [ChildRoles_ID] in table 'Roles_Roles'
ALTER TABLE [dbo].[Roles_Roles]
ADD CONSTRAINT [PK_Roles_Roles]
    PRIMARY KEY CLUSTERED ([ParentRoles_ID], [ChildRoles_ID] ASC);
GO

-- Creating primary key on [Products_Id], [Categories_Id] in table 'ProductsCategories'
ALTER TABLE [dbo].[ProductsCategories]
ADD CONSTRAINT [PK_ProductsCategories]
    PRIMARY KEY CLUSTERED ([Products_Id], [Categories_Id] ASC);
GO

-- Creating primary key on [Products_Id], [Tags_Id] in table 'ProductsTags'
ALTER TABLE [dbo].[ProductsTags]
ADD CONSTRAINT [PK_ProductsTags]
    PRIMARY KEY CLUSTERED ([Products_Id], [Tags_Id] ASC);
GO

-- Creating primary key on [Posts_Id], [Categories_Id] in table 'PostsCategories'
ALTER TABLE [dbo].[PostsCategories]
ADD CONSTRAINT [PK_PostsCategories]
    PRIMARY KEY CLUSTERED ([Posts_Id], [Categories_Id] ASC);
GO

-- Creating primary key on [Carts_Id], [Products_Id] in table 'CartsProducts'
ALTER TABLE [dbo].[CartsProducts]
ADD CONSTRAINT [PK_CartsProducts]
    PRIMARY KEY CLUSTERED ([Carts_Id], [Products_Id] ASC);
GO

-- Creating primary key on [Wishlists_Id], [Products_Id] in table 'WishlistsProducts'
ALTER TABLE [dbo].[WishlistsProducts]
ADD CONSTRAINT [PK_WishlistsProducts]
    PRIMARY KEY CLUSTERED ([Wishlists_Id], [Products_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Users_ID] in table 'Users_Roles'
ALTER TABLE [dbo].[Users_Roles]
ADD CONSTRAINT [FK_Users_Roles_User]
    FOREIGN KEY ([Users_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Roles_ID] in table 'Users_Roles'
ALTER TABLE [dbo].[Users_Roles]
ADD CONSTRAINT [FK_Users_Roles_Role]
    FOREIGN KEY ([Roles_ID])
    REFERENCES [dbo].[Roles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Users_Roles_Role'
CREATE INDEX [IX_FK_Users_Roles_Role]
ON [dbo].[Users_Roles]
    ([Roles_ID]);
GO

-- Creating foreign key on [Role_ID] in table 'TypePermissionObjects'
ALTER TABLE [dbo].[TypePermissionObjects]
ADD CONSTRAINT [FK_Role_TypePermissionObject]
    FOREIGN KEY ([Role_ID])
    REFERENCES [dbo].[Roles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Role_TypePermissionObject'
CREATE INDEX [IX_FK_Role_TypePermissionObject]
ON [dbo].[TypePermissionObjects]
    ([Role_ID]);
GO

-- Creating foreign key on [ParentRoles_ID] in table 'Roles_Roles'
ALTER TABLE [dbo].[Roles_Roles]
ADD CONSTRAINT [FK_Roles_Roles_Role1]
    FOREIGN KEY ([ParentRoles_ID])
    REFERENCES [dbo].[Roles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ChildRoles_ID] in table 'Roles_Roles'
ALTER TABLE [dbo].[Roles_Roles]
ADD CONSTRAINT [FK_Roles_Roles_Role2]
    FOREIGN KEY ([ChildRoles_ID])
    REFERENCES [dbo].[Roles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Roles_Roles_Role2'
CREATE INDEX [IX_FK_Roles_Roles_Role2]
ON [dbo].[Roles_Roles]
    ([ChildRoles_ID]);
GO

-- Creating foreign key on [Owner_ID] in table 'SecuritySystemMemberPermissionsObjects'
ALTER TABLE [dbo].[SecuritySystemMemberPermissionsObjects]
ADD CONSTRAINT [FK_TypePermissionObjectMemberPermissionsObject]
    FOREIGN KEY ([Owner_ID])
    REFERENCES [dbo].[TypePermissionObjects]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TypePermissionObjectMemberPermissionsObject'
CREATE INDEX [IX_FK_TypePermissionObjectMemberPermissionsObject]
ON [dbo].[SecuritySystemMemberPermissionsObjects]
    ([Owner_ID]);
GO

-- Creating foreign key on [Owner_ID] in table 'SecuritySystemObjectPermissionsObjects'
ALTER TABLE [dbo].[SecuritySystemObjectPermissionsObjects]
ADD CONSTRAINT [FK_TypePermissionObjectObjectPermissionsObject]
    FOREIGN KEY ([Owner_ID])
    REFERENCES [dbo].[TypePermissionObjects]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TypePermissionObjectObjectPermissionsObject'
CREATE INDEX [IX_FK_TypePermissionObjectObjectPermissionsObject]
ON [dbo].[SecuritySystemObjectPermissionsObjects]
    ([Owner_ID]);
GO

-- Creating foreign key on [Owner_ID] in table 'ModelDifferenceAspects'
ALTER TABLE [dbo].[ModelDifferenceAspects]
ADD CONSTRAINT [FK_ModelDifferenceAspect_ModelDifference]
    FOREIGN KEY ([Owner_ID])
    REFERENCES [dbo].[ModelDifferences]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModelDifferenceAspect_ModelDifference'
CREATE INDEX [IX_FK_ModelDifferenceAspect_ModelDifference]
ON [dbo].[ModelDifferenceAspects]
    ([Owner_ID]);
GO

-- Creating foreign key on [Product_type_id] in table 'ProductSet'
ALTER TABLE [dbo].[ProductSet]
ADD CONSTRAINT [FK_ProductsProduct_types]
    FOREIGN KEY ([Product_type_id])
    REFERENCES [dbo].[Product_typeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductsProduct_types'
CREATE INDEX [IX_FK_ProductsProduct_types]
ON [dbo].[ProductSet]
    ([Product_type_id]);
GO

-- Creating foreign key on [ProductsId] in table 'Product_detailSet'
ALTER TABLE [dbo].[Product_detailSet]
ADD CONSTRAINT [FK_Product_detailsProducts]
    FOREIGN KEY ([ProductsId])
    REFERENCES [dbo].[ProductSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_detailsProducts'
CREATE INDEX [IX_FK_Product_detailsProducts]
ON [dbo].[Product_detailSet]
    ([ProductsId]);
GO

-- Creating foreign key on [parent_id] in table 'CategorySet'
ALTER TABLE [dbo].[CategorySet]
ADD CONSTRAINT [FK_CategoriesCategories]
    FOREIGN KEY ([parent_id])
    REFERENCES [dbo].[CategorySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoriesCategories'
CREATE INDEX [IX_FK_CategoriesCategories]
ON [dbo].[CategorySet]
    ([parent_id]);
GO

-- Creating foreign key on [Category_type_id] in table 'CategorySet'
ALTER TABLE [dbo].[CategorySet]
ADD CONSTRAINT [FK_CategoriesCategory_types]
    FOREIGN KEY ([Category_type_id])
    REFERENCES [dbo].[Category_typeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoriesCategory_types'
CREATE INDEX [IX_FK_CategoriesCategory_types]
ON [dbo].[CategorySet]
    ([Category_type_id]);
GO

-- Creating foreign key on [Category_Id] in table 'Category_detailSet'
ALTER TABLE [dbo].[Category_detailSet]
ADD CONSTRAINT [FK_Category_detailsCategories]
    FOREIGN KEY ([Category_Id])
    REFERENCES [dbo].[CategorySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Category_detailsCategories'
CREATE INDEX [IX_FK_Category_detailsCategories]
ON [dbo].[Category_detailSet]
    ([Category_Id]);
GO

-- Creating foreign key on [Products_Id] in table 'ProductsCategories'
ALTER TABLE [dbo].[ProductsCategories]
ADD CONSTRAINT [FK_ProductsCategories_Products]
    FOREIGN KEY ([Products_Id])
    REFERENCES [dbo].[ProductSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Categories_Id] in table 'ProductsCategories'
ALTER TABLE [dbo].[ProductsCategories]
ADD CONSTRAINT [FK_ProductsCategories_Categories]
    FOREIGN KEY ([Categories_Id])
    REFERENCES [dbo].[CategorySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductsCategories_Categories'
CREATE INDEX [IX_FK_ProductsCategories_Categories]
ON [dbo].[ProductsCategories]
    ([Categories_Id]);
GO

-- Creating foreign key on [Products_Id] in table 'ProductsTags'
ALTER TABLE [dbo].[ProductsTags]
ADD CONSTRAINT [FK_ProductsTags_Products]
    FOREIGN KEY ([Products_Id])
    REFERENCES [dbo].[ProductSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Tags_Id] in table 'ProductsTags'
ALTER TABLE [dbo].[ProductsTags]
ADD CONSTRAINT [FK_ProductsTags_Tags]
    FOREIGN KEY ([Tags_Id])
    REFERENCES [dbo].[TagSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductsTags_Tags'
CREATE INDEX [IX_FK_ProductsTags_Tags]
ON [dbo].[ProductsTags]
    ([Tags_Id]);
GO

-- Creating foreign key on [post_id] in table 'Post_detailSet'
ALTER TABLE [dbo].[Post_detailSet]
ADD CONSTRAINT [FK_Post_detailsPosts]
    FOREIGN KEY ([post_id])
    REFERENCES [dbo].[PostSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Post_detailsPosts'
CREATE INDEX [IX_FK_Post_detailsPosts]
ON [dbo].[Post_detailSet]
    ([post_id]);
GO

-- Creating foreign key on [Posts_Id] in table 'PostsCategories'
ALTER TABLE [dbo].[PostsCategories]
ADD CONSTRAINT [FK_PostsCategories_Posts]
    FOREIGN KEY ([Posts_Id])
    REFERENCES [dbo].[PostSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Categories_Id] in table 'PostsCategories'
ALTER TABLE [dbo].[PostsCategories]
ADD CONSTRAINT [FK_PostsCategories_Categories]
    FOREIGN KEY ([Categories_Id])
    REFERENCES [dbo].[CategorySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PostsCategories_Categories'
CREATE INDEX [IX_FK_PostsCategories_Categories]
ON [dbo].[PostsCategories]
    ([Categories_Id]);
GO

-- Creating foreign key on [parent_id] in table 'DepartmentSet'
ALTER TABLE [dbo].[DepartmentSet]
ADD CONSTRAINT [FK_DepartmentsDepartments]
    FOREIGN KEY ([parent_id])
    REFERENCES [dbo].[DepartmentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentsDepartments'
CREATE INDEX [IX_FK_DepartmentsDepartments]
ON [dbo].[DepartmentSet]
    ([parent_id]);
GO

-- Creating foreign key on [block_id] in table 'FloorSet'
ALTER TABLE [dbo].[FloorSet]
ADD CONSTRAINT [FK_FloorsDepartments]
    FOREIGN KEY ([block_id])
    REFERENCES [dbo].[DepartmentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FloorsDepartments'
CREATE INDEX [IX_FK_FloorsDepartments]
ON [dbo].[FloorSet]
    ([block_id]);
GO

-- Creating foreign key on [floor_id] in table 'RoomSet'
ALTER TABLE [dbo].[RoomSet]
ADD CONSTRAINT [FK_RoomsFloors]
    FOREIGN KEY ([floor_id])
    REFERENCES [dbo].[FloorSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoomsFloors'
CREATE INDEX [IX_FK_RoomsFloors]
ON [dbo].[RoomSet]
    ([floor_id]);
GO

-- Creating foreign key on [room_id] in table 'CanvaSet'
ALTER TABLE [dbo].[CanvaSet]
ADD CONSTRAINT [FK_CanvasRooms]
    FOREIGN KEY ([room_id])
    REFERENCES [dbo].[RoomSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CanvasRooms'
CREATE INDEX [IX_FK_CanvasRooms]
ON [dbo].[CanvaSet]
    ([room_id]);
GO

-- Creating foreign key on [cart_id] in table 'CanvaSet'
ALTER TABLE [dbo].[CanvaSet]
ADD CONSTRAINT [FK_CartsCanvas]
    FOREIGN KEY ([cart_id])
    REFERENCES [dbo].[CartSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CartsCanvas'
CREATE INDEX [IX_FK_CartsCanvas]
ON [dbo].[CanvaSet]
    ([cart_id]);
GO

-- Creating foreign key on [Carts_Id] in table 'CartsProducts'
ALTER TABLE [dbo].[CartsProducts]
ADD CONSTRAINT [FK_CartsProducts_Carts]
    FOREIGN KEY ([Carts_Id])
    REFERENCES [dbo].[CartSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Products_Id] in table 'CartsProducts'
ALTER TABLE [dbo].[CartsProducts]
ADD CONSTRAINT [FK_CartsProducts_Products]
    FOREIGN KEY ([Products_Id])
    REFERENCES [dbo].[ProductSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CartsProducts_Products'
CREATE INDEX [IX_FK_CartsProducts_Products]
ON [dbo].[CartsProducts]
    ([Products_Id]);
GO

-- Creating foreign key on [user_id] in table 'CartSet'
ALTER TABLE [dbo].[CartSet]
ADD CONSTRAINT [FK_UserCarts]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserCarts'
CREATE INDEX [IX_FK_UserCarts]
ON [dbo].[CartSet]
    ([user_id]);
GO

-- Creating foreign key on [user_id] in table 'CanvaSet'
ALTER TABLE [dbo].[CanvaSet]
ADD CONSTRAINT [FK_UserCanvas]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserCanvas'
CREATE INDEX [IX_FK_UserCanvas]
ON [dbo].[CanvaSet]
    ([user_id]);
GO

-- Creating foreign key on [author_id] in table 'ProductSet'
ALTER TABLE [dbo].[ProductSet]
ADD CONSTRAINT [FK_UserProducts]
    FOREIGN KEY ([author_id])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserProducts'
CREATE INDEX [IX_FK_UserProducts]
ON [dbo].[ProductSet]
    ([author_id]);
GO

-- Creating foreign key on [author_id] in table 'PostSet'
ALTER TABLE [dbo].[PostSet]
ADD CONSTRAINT [FK_UserPosts]
    FOREIGN KEY ([author_id])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPosts'
CREATE INDEX [IX_FK_UserPosts]
ON [dbo].[PostSet]
    ([author_id]);
GO

-- Creating foreign key on [user_id] in table 'WishlistSet'
ALTER TABLE [dbo].[WishlistSet]
ADD CONSTRAINT [FK_UserWishlists]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserWishlists'
CREATE INDEX [IX_FK_UserWishlists]
ON [dbo].[WishlistSet]
    ([user_id]);
GO

-- Creating foreign key on [Wishlists_Id] in table 'WishlistsProducts'
ALTER TABLE [dbo].[WishlistsProducts]
ADD CONSTRAINT [FK_WishlistsProducts_Wishlists]
    FOREIGN KEY ([Wishlists_Id])
    REFERENCES [dbo].[WishlistSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Products_Id] in table 'WishlistsProducts'
ALTER TABLE [dbo].[WishlistsProducts]
ADD CONSTRAINT [FK_WishlistsProducts_Products]
    FOREIGN KEY ([Products_Id])
    REFERENCES [dbo].[ProductSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WishlistsProducts_Products'
CREATE INDEX [IX_FK_WishlistsProducts_Products]
ON [dbo].[WishlistsProducts]
    ([Products_Id]);
GO

-- Creating foreign key on [parent_id] in table 'PostSet'
ALTER TABLE [dbo].[PostSet]
ADD CONSTRAINT [FK_PostPost]
    FOREIGN KEY ([parent_id])
    REFERENCES [dbo].[PostSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PostPost'
CREATE INDEX [IX_FK_PostPost]
ON [dbo].[PostSet]
    ([parent_id]);
GO

-- Creating foreign key on [User_ID] in table 'CustomerSet'
ALTER TABLE [dbo].[CustomerSet]
ADD CONSTRAINT [FK_CustomerUser]
    FOREIGN KEY ([User_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerUser'
CREATE INDEX [IX_FK_CustomerUser]
ON [dbo].[CustomerSet]
    ([User_ID]);
GO

-- Creating foreign key on [customer_type_id] in table 'CustomerSet'
ALTER TABLE [dbo].[CustomerSet]
ADD CONSTRAINT [FK_CustomerCustomer_type]
    FOREIGN KEY ([customer_type_id])
    REFERENCES [dbo].[Customer_typeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCustomer_type'
CREATE INDEX [IX_FK_CustomerCustomer_type]
ON [dbo].[CustomerSet]
    ([customer_type_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------