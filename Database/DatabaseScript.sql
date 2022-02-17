create table AspNetRoles
(
    Id               nvarchar(450) not null,
    Name             nvarchar(256),
    NormalizedName   nvarchar(256),
    ConcurrencyStamp nvarchar(max),
    constraint PK_AspNetRoles
        primary key clustered (Id asc)
)
go

create table AspNetRoleClaims
(
    Id         int identity,
    RoleId     nvarchar(450) not null,
    ClaimType  nvarchar(max),
    ClaimValue nvarchar(max),
    constraint PK_AspNetRoleClaims
        primary key clustered (Id asc),
    constraint FK_AspNetRoleClaims_AspNetRoles_RoleId
        foreign key (RoleId) references AspNetRoles (Id)
            on delete cascade
)
go

create index IX_AspNetRoleClaims_RoleId
    on AspNetRoleClaims (RoleId)
go

create unique index RoleNameIndex
    on AspNetRoles (NormalizedName)
    where [NormalizedName] IS NOT NULL
go

create table AspNetUsers
(
    Id                   nvarchar(450) not null,
    UserName             nvarchar(256),
    NormalizedUserName   nvarchar(256),
    Email                nvarchar(256),
    NormalizedEmail      nvarchar(256),
    EmailConfirmed       bit           not null,
    PasswordHash         nvarchar(max),
    SecurityStamp        nvarchar(max),
    ConcurrencyStamp     nvarchar(max),
    PhoneNumber          nvarchar(max),
    PhoneNumberConfirmed bit           not null,
    TwoFactorEnabled     bit           not null,
    LockoutEnd           datetimeoffset,
    LockoutEnabled       bit           not null,
    AccessFailedCount    int           not null,
    constraint PK_AspNetUsers
        primary key clustered (Id asc)
)
go

create table AspNetUserClaims
(
    Id         int identity,
    UserId     nvarchar(450) not null,
    ClaimType  nvarchar(max),
    ClaimValue nvarchar(max),
    constraint PK_AspNetUserClaims
        primary key clustered (Id asc),
    constraint FK_AspNetUserClaims_AspNetUsers_UserId
        foreign key (UserId) references AspNetUsers (Id)
            on delete cascade
)
go

create index IX_AspNetUserClaims_UserId
    on AspNetUserClaims (UserId)
go

create table AspNetUserLogins
(
    LoginProvider       nvarchar(128) not null,
    ProviderKey         nvarchar(128) not null,
    ProviderDisplayName nvarchar(max),
    UserId              nvarchar(450) not null,
    constraint PK_AspNetUserLogins
        primary key clustered (LoginProvider asc, ProviderKey asc),
    constraint FK_AspNetUserLogins_AspNetUsers_UserId
        foreign key (UserId) references AspNetUsers (Id)
            on delete cascade
)
go

create index IX_AspNetUserLogins_UserId
    on AspNetUserLogins (UserId)
go

create table AspNetUserRoles
(
    UserId nvarchar(450) not null,
    RoleId nvarchar(450) not null,
    constraint PK_AspNetUserRoles
        primary key clustered (UserId asc, RoleId asc),
    constraint FK_AspNetUserRoles_AspNetRoles_RoleId
        foreign key (RoleId) references AspNetRoles (Id)
            on delete cascade,
    constraint FK_AspNetUserRoles_AspNetUsers_UserId
        foreign key (UserId) references AspNetUsers (Id)
            on delete cascade
)
go

create index IX_AspNetUserRoles_RoleId
    on AspNetUserRoles (RoleId)
go

create table AspNetUserTokens
(
    UserId        nvarchar(450) not null,
    LoginProvider nvarchar(128) not null,
    Name          nvarchar(128) not null,
    Value         nvarchar(max),
    constraint PK_AspNetUserTokens
        primary key clustered (UserId asc, LoginProvider asc, Name asc),
    constraint FK_AspNetUserTokens_AspNetUsers_UserId
        foreign key (UserId) references AspNetUsers (Id)
            on delete cascade
)
go

create index EmailIndex
    on AspNetUsers (NormalizedEmail)
go

create unique index UserNameIndex
    on AspNetUsers (NormalizedUserName)
    where [NormalizedUserName] IS NOT NULL
go

create table Categories
(
    Id   int identity,
    Name nvarchar(50) not null,
    constraint PK_Categories
        primary key clustered (Id asc)
)
go

create table States
(
    Id           int identity,
    Name         nvarchar(50) not null,
    Description  nchar(100)   not null,
    ColorHexCode nvarchar(7)  not null,
    constraint PK_States
        primary key clustered (Id asc)
)
go

create table Suggestions
(
    Id            int identity,
    Title         nchar(100)    not null,
    Description   text,
    UpvotesAmount int           not null,
    CategoryId    int           not null,
    StateId       int,
    UserId        nvarchar(450) not null,
    Date          datetime      not null,
    Approved      bit,
    constraint PK_Suggestions
        primary key clustered (Id asc),
    constraint FK_Categories_Suggestions
        foreign key (CategoryId) references Categories (Id),
    constraint FK_States_Suggestions
        foreign key (StateId) references States (Id)
)
go

create table Upvotes
(
    Id           int identity,
    SuggestionId int           not null,
    UserId       nvarchar(450) not null,
    constraint PK_Upvotes
        primary key clustered (Id asc),
    constraint FK_Suggestions_Upvotes
        foreign key (SuggestionId) references Suggestions (Id)
)
go

create table __EFMigrationsHistory
(
    MigrationId    nvarchar(150) not null,
    ProductVersion nvarchar(32)  not null,
    constraint PK___EFMigrationsHistory
        primary key clustered (MigrationId asc)
)
go


