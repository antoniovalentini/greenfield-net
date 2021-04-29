# Executor
A tool which allows you to chain a sequence of synchronous operations. You can think of it as "synchronous promises".

## Example of use

```C#
var result = new Executor<int>(CalculateNumber())
    .Then(a => $"a is an int: {a}.")
    .Then(b => DateTime.Now)
    .Then(c => true)
    .Then(() => _output.WriteLine(""))
    .Catch(ex => _output.WriteLine($"Exception raised: {ex.Message}"));
```

# Swissknife
A set of common tools.
