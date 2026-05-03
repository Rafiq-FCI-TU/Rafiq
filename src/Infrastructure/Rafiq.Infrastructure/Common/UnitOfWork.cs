//using Rafiq.Application.Common;
//using Rafiq.Domain.Entities;
//using Rafiq.Persistence.Data;

//namespace Rafiq.Infrastructure.Common;

//public class UnitOfWork : IUnitOfWork
//{
//    private readonly RafiqDbContext _context;

//    private IRepository<Category>? _categories;
//    private IRepository<Game>? _games;
//    private IRepository<GameLevel>? _gameLevels;
//    private IRepository<GameQuestion>? _gameQuestions;
//    private IRepository<GameOption>? _gameOptions;
//    private IRepository<PatientLevel>? _patientLevels;
//    private IRepository<Patient>? _patients;

//    public UnitOfWork(RafiqDbContext context)
//    {
//        _context = context;
//    }

//    public IRepository<Category> Categories => _categories ??= new Repository<Category>(_context);

//    public IRepository<Game> Games => _games ??= new Repository<Game>(_context);

//    public IRepository<GameLevel> GameLevels => _gameLevels ??= new Repository<GameLevel>(_context);

//    public IRepository<GameQuestion> GameQuestions => _gameQuestions ??= new Repository<GameQuestion>(_context);

//    public IRepository<GameOption> GameOptions => _gameOptions ??= new Repository<GameOption>(_context);

//    public IRepository<PatientLevel> PatientLevels => _patientLevels ??= new Repository<PatientLevel>(_context);

//    public IRepository<Patient> Patients => _patients ??= new Repository<Patient>(_context);

//    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
//        => await _context.SaveChangesAsync(cancellationToken);

//    public void Dispose() => _context.Dispose();
//}
