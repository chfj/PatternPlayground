// Several tests redirect Console.Out to capture what a Demo/Send/Print
// method prints. Console.Out is process-wide mutable state, so if xUnit
// ran different test classes in parallel (its default), two classes
// redirecting it at the same time could stomp on each other and produce
// flaky failures. Running the whole assembly sequentially avoids that.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
