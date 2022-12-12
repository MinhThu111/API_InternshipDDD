using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Services;
using API.Internship.Infrastructure.Data;
using API.Internship.Infrastructure.Repositories;
using API.Internship.OPS.Helper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<InternshipContext>();

builder.Services.AddTransient<IGradeRepository, GradeRepository>();
builder.Services.AddTransient<IGradeService, GradeService>();
builder.Services.AddTransient<IGradeHelper, GradeHelper>();


//person
builder.Services.AddTransient<IPersonRepository, PersonRepository>();
builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddTransient<IPersonHelper, PersonHelper>();

//persontype
builder.Services.AddTransient<IPersonTypeRepository, PersonTypeRepository>();
builder.Services.AddTransient<IPersonTypeService, PersonTypeService>();
builder.Services.AddTransient<IPersonTypeHelper, PersonTypeHelper>();

//address
builder.Services.AddTransient<IAddressRepository, AddressRepository>();
builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddTransient<IAddressHelper, AddressHelper>();


//country
builder.Services.AddTransient<ICountryRepository, CountryRepository>();
builder.Services.AddTransient<ICountryService, CountryService>();
builder.Services.AddTransient<ICountryHelper, CountryHelper>();


//province
builder.Services.AddTransient<IProvinceRepository, ProvinceRepository>();
builder.Services.AddTransient<IProvinceService, ProvinceSerivce>();
builder.Services.AddTransient<IProvinceHelper, ProvinceHelper>();


//district
builder.Services.AddTransient<IDistrictRepository, DistrictRepository>();
builder.Services.AddTransient<IDistrictService, DistrictService>();
builder.Services.AddTransient<IDistrictHelper, DistrictHelper>();

//ward
builder.Services.AddTransient<IWardRepository, WardRepository>();
builder.Services.AddTransient<IWardService, WardService>();
builder.Services.AddTransient<IWardHelper, WardHelper>();

//nationality
builder.Services.AddTransient<INationalityRepository, NationalityRepository>();
builder.Services.AddTransient<INationalityService, NationalityService>();
builder.Services.AddTransient<INationalityHelper, NationalityHelper>();

//religion
builder.Services.AddTransient<IReligionRepository, ReligionRepository>();
builder.Services.AddTransient<IReligionService, ReligionService>();
builder.Services.AddTransient<IReligionHelper, ReligionHelper>();

//folk
builder.Services.AddTransient<IFolkRepository, FolkRepository>();
builder.Services.AddTransient<IFolkService, FolkService>();
builder.Services.AddTransient<IFolkHelper, FolkHelper>();

//-GradeStudent
builder.Services.AddTransient<IGradeStudentRepository, GradeStudentRepository>();
builder.Services.AddTransient<IGradeStudentService, GradeStudentService>();
builder.Services.AddTransient<IGradeStudentHelper, GradeStudentHelper>();

//- News
builder.Services.AddTransient<INewsRepository, NewsRepository>();
builder.Services.AddTransient<INewsService, NewsService>();
builder.Services.AddTransient<INewsHelper, NewsHelper>();

//- NewsCategory
builder.Services.AddTransient<INewsCategoryRepository, NewsCategoryRepository>();
builder.Services.AddTransient<INewsCategoryService, NewsCategoryService>();
builder.Services.AddTransient<INewsCategoryHelper, NewsCategoryHelper>();

//- ParentContact
builder.Services.AddTransient<IParentContactRepository, ParentContactRepository>();
builder.Services.AddTransient<IParentContactService, ParentContactService>();
builder.Services.AddTransient<IParentContactHelper, ParentContactHelper>();

//- Position
builder.Services.AddTransient<IPositionRepository, PositionRepository>();
builder.Services.AddTransient<IPositionService, PositionService>();
builder.Services.AddTransient<IPositionHelper, PositionHelper>();

//- SchoolInfo
builder.Services.AddTransient<ISchoolInfoRepository, SchoolInfoRepository>();
builder.Services.AddTransient<ISchoolInfoService, SchoolInfoService>();
builder.Services.AddTransient<ISchoolInfoHelper, SchoolInfoHelper>();

//- Score
builder.Services.AddTransient<IScoreRepository, ScoreRepository>();
builder.Services.AddTransient<IScoreService, ScoreService>();
builder.Services.AddTransient<IScoreHelper, ScoreHelper>();

//- ScoreType
builder.Services.AddTransient<IScoreTypeRepository, ScoreTypeRepository>();
builder.Services.AddTransient<IScoreTypeService, ScoreTypeService>();
builder.Services.AddTransient<IScoreTypeHelper, ScoreTypeHelper>();

//- StudentParentContact
builder.Services.AddTransient<IStudentParentContactRepository, StudentParentContactRepository>();
builder.Services.AddTransient<IStudentParentContactService, StudentParentContactService>();
builder.Services.AddTransient<IStudentParentContactHelper, StudentParentContactHelper>();

//- Subject
builder.Services.AddTransient<ISubjectRepository, SubjectRepository>();
builder.Services.AddTransient<ISubjectService, SubjectService>();
builder.Services.AddTransient<ISubjectHelper, SubjectHelper>();

//- TeacherSubject
builder.Services.AddTransient<ITeacherSubjectRepository, TeacherSubjectRepository>();
builder.Services.AddTransient<ITeacherSubjectService, TeacherSubjectService>();
builder.Services.AddTransient<ITeacherSubjectHelper, TeacherSubjectHelper>();

//var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddResponseCompression();

builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    //options.SuppressConsumesConstraintForFormFileParameters = true;//Multipart/form-data request inference
    options.SuppressInferBindingSourcesForParameters = true; //Disable inference rules

    //options.SuppressModelStateInvalidFilter = true;
    //options.SuppressMapClientErrors = true; //Disable ProblemDetails
    //options.ClientErrorMapping[StatusCodes.Status404NotFound].Link = "https://httpstatuses.com/404";
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
       builder =>
       {
           //builder.WithOrigins("http://example.com", "http://www.contoso.com");
           builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetPreflightMaxAge(TimeSpan.FromSeconds(300));
       });
});
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API.Internship.OPS", Version = "v1" });
});

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddOptions();
builder.Services.AddMvc();// Add framework services.
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger(c =>
    {
        //you can opt into exposing JSON in the 2.0 format instead
        c.SerializeAsV2 = false;// Enable middleware to serve generated Swagger as a JSON endpoint
        c.RouteTemplate = "api-docs/{documentName}/swagger.json";
    });
    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.    
    app.UseSwaggerUI(c =>
    {
        //c.RoutePrefix = "swagger"; //To serve the Swagger UI at the app's root
        //c.SwaggerEndpoint("../swagger/v1/swagger.json", "API.BDSGiaPhu.OPS v1");
        c.SwaggerEndpoint("/api-docs/v1/swagger.json", "API.Internship.OPS v1");

        c.DefaultModelExpandDepth(2);
        c.DefaultModelRendering(ModelRendering.Model);
        c.DefaultModelsExpandDepth(-1);
        c.DisplayOperationId();
        c.DisplayRequestDuration();
        c.DocExpansion(DocExpansion.None);
        c.EnableDeepLinking();
        c.EnableFilter();
        //c.MaxDisplayedTags(5);
        c.ShowExtensions();
        c.ShowCommonExtensions();
        c.EnableValidator();
        //c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head);
        c.UseRequestInterceptor("(request) => { return request; }");
        c.UseResponseInterceptor("(response) => { return response; }");
    });
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
