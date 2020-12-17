# EFCoreWithNoLock
You can read data from DataBase by **EntityFrameworkCore** when Table is locked by another process. in this case you read  **UnCommited** data from DataBase

# Example
```csharp
var categories = dbContext.Categories
                          .AsNoTracking()
                          .Where(a => a.IsDelete == false)
                          .ToListWithNoLockAsync();
```
