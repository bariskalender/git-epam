CREATE TABLE Person (
    PersonId INTEGER PRIMARY KEY,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL
);

CREATE TABLE PhoneType (
    PhoneTypeId INTEGER PRIMARY KEY,
    PhoneTypeName TEXT NOT NULL UNIQUE
);

CREATE TABLE Position (
    PositionId INTEGER PRIMARY KEY,
    PositionName TEXT NOT NULL UNIQUE
);

CREATE TABLE Manufacturer (
    ManufacturerId INTEGER PRIMARY KEY,
    ManufacturerName TEXT NOT NULL UNIQUE
);

CREATE TABLE MatrixType (
    MatrixTypeId INTEGER PRIMARY KEY,
    MatrixTypeName TEXT NOT NULL UNIQUE
);

CREATE TABLE ScreenResolution (
    ScreenResolutionId INTEGER PRIMARY KEY,
    ResolutionName TEXT NOT NULL UNIQUE
);

CREATE TABLE RamSize (
    RamSizeId INTEGER PRIMARY KEY,
    RamSizeName TEXT NOT NULL UNIQUE
);

CREATE TABLE RomSize (
    RomSizeId INTEGER PRIMARY KEY,
    RomSizeName TEXT NOT NULL UNIQUE
);

CREATE TABLE ManufactureYear (
    ManufactureYearId INTEGER PRIMARY KEY,
    YearValue INTEGER NOT NULL UNIQUE
);

CREATE TABLE Malfunction (
    MalfunctionId INTEGER PRIMARY KEY,
    MalfunctionDescription TEXT NOT NULL UNIQUE
);

CREATE TABLE RepairState (
    RepairStateId INTEGER PRIMARY KEY,
    StateName TEXT NOT NULL UNIQUE
);

CREATE TABLE Owner (
    OwnerId INTEGER PRIMARY KEY,
    PersonId INTEGER NOT NULL UNIQUE,
    FOREIGN KEY (PersonId) REFERENCES Person(PersonId)
);

CREATE TABLE Brand (
    BrandId INTEGER PRIMARY KEY,
    ManufacturerId INTEGER NOT NULL,
    BrandName TEXT NOT NULL,
    UNIQUE (ManufacturerId, BrandName),
    FOREIGN KEY (ManufacturerId) REFERENCES Manufacturer(ManufacturerId)
);

CREATE TABLE OwnerPhone (
    OwnerPhoneId INTEGER PRIMARY KEY,
    OwnerId INTEGER NOT NULL,
    PhoneTypeId INTEGER NOT NULL,
    PhoneNumber TEXT NOT NULL UNIQUE,
    FOREIGN KEY (OwnerId) REFERENCES Owner(OwnerId),
    FOREIGN KEY (PhoneTypeId) REFERENCES PhoneType(PhoneTypeId)
);

CREATE TABLE Employee (
    EmployeeId INTEGER PRIMARY KEY,
    PersonId INTEGER NOT NULL UNIQUE,
    PositionId INTEGER NOT NULL,
    FOREIGN KEY (PersonId) REFERENCES Person(PersonId),
    FOREIGN KEY (PositionId) REFERENCES Position(PositionId)
);

CREATE TABLE DisplaySpecification (
    DisplaySpecificationId INTEGER PRIMARY KEY,
    ScreenResolutionId INTEGER NOT NULL,
    MatrixTypeId INTEGER NOT NULL,
    UNIQUE (ScreenResolutionId, MatrixTypeId),
    FOREIGN KEY (ScreenResolutionId) REFERENCES ScreenResolution(ScreenResolutionId),
    FOREIGN KEY (MatrixTypeId) REFERENCES MatrixType(MatrixTypeId)
);

CREATE TABLE StorageSpecification (
    StorageSpecificationId INTEGER PRIMARY KEY,
    RamSizeId INTEGER NOT NULL,
    RomSizeId INTEGER NOT NULL,
    UNIQUE (RamSizeId, RomSizeId),
    FOREIGN KEY (RamSizeId) REFERENCES RamSize(RamSizeId),
    FOREIGN KEY (RomSizeId) REFERENCES RomSize(RomSizeId)
);

CREATE TABLE SmartphoneModel (
    SmartphoneModelId INTEGER PRIMARY KEY,
    BrandId INTEGER NOT NULL,
    ModelName TEXT NOT NULL,
    UNIQUE (BrandId, ModelName),
    FOREIGN KEY (BrandId) REFERENCES Brand(BrandId)
);

CREATE TABLE Smartphone (
    SmartphoneId INTEGER PRIMARY KEY,
    SmartphoneModelId INTEGER NOT NULL,
    DisplaySpecificationId INTEGER NOT NULL,
    StorageSpecificationId INTEGER NOT NULL,
    ManufactureYearId INTEGER NOT NULL,
    FOREIGN KEY (SmartphoneModelId) REFERENCES SmartphoneModel(SmartphoneModelId),
    FOREIGN KEY (DisplaySpecificationId) REFERENCES DisplaySpecification(DisplaySpecificationId),
    FOREIGN KEY (StorageSpecificationId) REFERENCES StorageSpecification(StorageSpecificationId),
    FOREIGN KEY (ManufactureYearId) REFERENCES ManufactureYear(ManufactureYearId)
);

CREATE TABLE ImeiRecord (
    ImeiRecordId INTEGER PRIMARY KEY,
    SmartphoneId INTEGER NOT NULL UNIQUE,
    ImeiPrimary TEXT NOT NULL UNIQUE,
    ImeiSecondary TEXT UNIQUE,
    FOREIGN KEY (SmartphoneId) REFERENCES Smartphone(SmartphoneId)
);

CREATE TABLE RepairCase (
    RepairCaseId INTEGER PRIMARY KEY,
    SmartphoneId INTEGER NOT NULL,
    OwnerPhoneId INTEGER NOT NULL,
    AcceptedByEmployeeId INTEGER NOT NULL,
    MalfunctionId INTEGER NOT NULL,
    FOREIGN KEY (SmartphoneId) REFERENCES Smartphone(SmartphoneId),
    FOREIGN KEY (OwnerPhoneId) REFERENCES OwnerPhone(OwnerPhoneId),
    FOREIGN KEY (AcceptedByEmployeeId) REFERENCES Employee(EmployeeId),
    FOREIGN KEY (MalfunctionId) REFERENCES Malfunction(MalfunctionId)
);

CREATE TABLE Receipt (
    ReceiptId INTEGER PRIMARY KEY,
    RepairCaseId INTEGER NOT NULL UNIQUE,
    ReceiptNumber TEXT NOT NULL UNIQUE,
    FOREIGN KEY (RepairCaseId) REFERENCES RepairCase(RepairCaseId)
);

CREATE TABLE RepairOrder (
    RepairOrderId INTEGER PRIMARY KEY,
    ReceiptId INTEGER NOT NULL UNIQUE,
    RepairEmployeeId INTEGER NOT NULL,
    RepairStateId INTEGER NOT NULL,
    MalfunctionId INTEGER NOT NULL,
    FOREIGN KEY (ReceiptId) REFERENCES Receipt(ReceiptId),
    FOREIGN KEY (RepairEmployeeId) REFERENCES Employee(EmployeeId),
    FOREIGN KEY (RepairStateId) REFERENCES RepairState(RepairStateId),
    FOREIGN KEY (MalfunctionId) REFERENCES Malfunction(MalfunctionId)
);

CREATE TABLE Payment (
    PaymentId INTEGER PRIMARY KEY,
    RepairOrderId INTEGER NOT NULL UNIQUE,
    AmountToPay REAL NOT NULL,
    FOREIGN KEY (RepairOrderId) REFERENCES RepairOrder(RepairOrderId)
);