﻿using UnitTestInPractice.Domain.Root;

namespace UnitTestInPractice.Infrastructure;


public class UnitTestInPracticeDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;
    public UnitTestInPracticeDbContext(DbContextOptions<UnitTestInPracticeDbContext> options) : base(options) { }

    public UnitTestInPracticeDbContext(DbContextOptions<UnitTestInPracticeDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
    }
    public DbSet<Assessment> Assessments { get; set; } = null!;   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        await _mediator.DispatchDomainEventsAsync(this);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed
        var result = await base.SaveChangesAsync(cancellationToken);

        return true;
    }


}

