
using API.Internship.Domain.Interfaces;
using API.Internship.Infrastructure.Repositories;

namespace API.Internship.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InternshipContext _context;
        public UnitOfWork(InternshipContext context)
        {
            _context = context;
            //jobRepository = jobRepository ?? new JobRepository(_context);
        }
        public IGradeRepository gradeRepository;
        public IGradeRepository GradeRepository
        {
            get
            {
                return gradeRepository = gradeRepository ?? new GradeRepository(_context);
            }
        }

        //person
        public IPersonRepository personRepository;
        public IPersonRepository PersonRepository
        {
            get
            {
                return personRepository = personRepository ?? new PersonRepository(_context);
            }
        }

        //persontype
        public IPersonTypeRepository persontypeRepository;
        public IPersonTypeRepository PersonTypeRepository
        {
            get
            {
                return persontypeRepository = persontypeRepository ?? new PersonTypeRepository(_context);
            }
        }

        //address
        public IAddressRepository addressRepository;
        public IAddressRepository AddressRepository
        {
            get
            {
                return addressRepository = addressRepository ?? new AddressRepository(_context);
            }
        }
        
        //country
        public ICountryRepository countryRepository;
        public ICountryRepository CountryRepository
        {
            get
            {
                return countryRepository = countryRepository ?? new CountryRepository(_context);
            }
        }

        //ward
        public IWardRepository wardRepository;
        public IWardRepository WardRepository
        {
            get
            {
                return wardRepository = wardRepository ?? new WardRepository(_context);
            }
        }

        //Province
        public IProvinceRepository provinceRepository;
        public IProvinceRepository ProvinceRepository
        {
            get
            {
                return provinceRepository = provinceRepository ?? new ProvinceRepository(_context);
            }
        }

        //District
        public IDistrictRepository districtRepository;
        public IDistrictRepository DistrictRepository
        {
            get
            {
                return districtRepository = districtRepository ?? new DistrictRepository(_context);
            }
        }

        //Nationality
        public INationalityRepository nationalityRepository;
        public INationalityRepository NationalityRepository
        {
            get
            {
                return nationalityRepository = nationalityRepository ?? new NationalityRepository(_context);
            }
        }

        //folk
        public IFolkRepository folkRepository;
        public IFolkRepository FolkRepository
        {
            get
            {
                return folkRepository = folkRepository ?? new FolkRepository(_context);
            }
        }

        //religion
        public IReligionRepository religionRepository;
        public IReligionRepository ReligionRepository
        {
            get
            {
                return religionRepository = religionRepository ?? new ReligionRepository(_context);
            }
        }

        public IGradeStudentRepository gradeStudentRepository;
        public IGradeStudentRepository GradeStudentRepository { get 
            { return gradeStudentRepository = gradeStudentRepository ?? new GradeStudentRepository(_context); } }
        public INewsRepository newsRepository;
        public INewsRepository NewsRepository { get { return newsRepository = newsRepository ?? new NewsRepository(_context); } }
        public INewsCategoryRepository newsCategoryRepository;
        public INewsCategoryRepository NewsCategoryRepository { get { return newsCategoryRepository = newsCategoryRepository ?? new NewsCategoryRepository(_context); } }
        public IParentContactRepository parentContactRepository;
        public IParentContactRepository ParentContactRepository { get { return parentContactRepository = parentContactRepository ?? new ParentContactRepository(_context); } }
        public IPositionRepository positionRepository;
        public IPositionRepository PositionRepository { get { return positionRepository = positionRepository ?? new PositionRepository(_context); } }
        public ISchoolInfoRepository schoolInfoRepository;
        public ISchoolInfoRepository SchoolInfoRepository { get { return schoolInfoRepository = schoolInfoRepository ?? new SchoolInfoRepository(_context); } }
        public IScoreRepository scoreRepository;
        public IScoreRepository ScoreRepository { get { return scoreRepository = scoreRepository ?? new ScoreRepository(_context); } }
        public IScoreTypeRepository scoreTypeRepository;
        public IScoreTypeRepository ScoreTypeRepository { get { return scoreTypeRepository = scoreTypeRepository ?? new ScoreTypeRepository(_context); } }
        public IStudentParentContactRepository studentParentContactRepository;
        public IStudentParentContactRepository StudentParentContactRepository { get { return studentParentContactRepository = studentParentContactRepository ?? new StudentParentContactRepository(_context); } }
        public ISubjectRepository subjectRepository;
        public ISubjectRepository SubjectRepository { get { return subjectRepository = subjectRepository ?? new SubjectRepository(_context); } }
        public ITeacherSubjectRepository teacherSubjectRepository;
        public ITeacherSubjectRepository TeacherSubjectRepository
        {
            get { return teacherSubjectRepository = teacherSubjectRepository ?? new TeacherSubjectRepository(_context); }
        }
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task RollbackAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
