namespace API.Internship.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGradeRepository GradeRepository { get; }
        IPersonRepository PersonRepository { get; }
        IPersonTypeRepository PersonTypeRepository { get; }
        IAddressRepository AddressRepository { get; }
        ICountryRepository CountryRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        IDistrictRepository DistrictRepository { get; }
        IWardRepository WardRepository { get; }
        INationalityRepository NationalityRepository { get; }
        IReligionRepository ReligionRepository { get; }
        IFolkRepository FolkRepository { get; }


        IGradeStudentRepository GradeStudentRepository { get; }
        INewsRepository NewsRepository { get; }
        INewsCategoryRepository NewsCategoryRepository { get; }
        IParentContactRepository ParentContactRepository { get; }
        IPositionRepository PositionRepository { get; }
        ISchoolInfoRepository SchoolInfoRepository { get; }
        IScoreRepository ScoreRepository { get; }
        IScoreTypeRepository ScoreTypeRepository { get; }
        IStudentParentContactRepository StudentParentContactRepository { get; }
        ISubjectRepository SubjectRepository { get; }
        ITeacherSubjectRepository TeacherSubjectRepository { get; }



        Task<int> CommitAsync();
        Task RollbackAsync();
    }
}
