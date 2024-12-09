namespace AoC.Year2024;

public partial class Day09(
    ILogger<Day09> _logger,
    AdventOfCodeClient _client,
    AocHelper _helper)
    : IDay
{
    private readonly ILogger<Day09> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));
    private readonly AocHelper _helper = _helper ?? throw new ArgumentNullException(nameof(_helper));

    private static long[] PreAllocateMemory(List<int> data)
    {
        //preallocate memory
        var memory = new long[data.Sum()];
        var memoryIndex = 0;
        var valueItterator = 0;

        for (var dataIndex = 0; dataIndex < data.Count; dataIndex++)
        {
            var value = dataIndex % 2 == 0 ? valueItterator++ : -1;
            var current = data[dataIndex];
            for (var j = 0; j < current; j++)
            {
                memory[memoryIndex] = value;
                memoryIndex++;
            }
        }

        return memory;
    }

    private static long CalculateChecksum(long[] memory)
    {
        var result = 0L;
        for (var i = 0; i < memory.Length; i++)
            if (memory[i] != -1)
                result += memory[i] * i;
        return result;
    }

    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 9);

        //split data every character
        var data = input
            .SelectMany(x => x.ToString())
            .Select(x => int.Parse(x.ToString()))
            .ToList();

        //preallocate memory
        var memory = PreAllocateMemory(data);

        //use duble pointer technique to swap -1 with none -1
        var endingPointer = memory.Length - 1;
        var startingPointer = 0;
        var swap = 0L;
        while (true)
        {
            //move starting pointer to the next -1
            while (startingPointer < memory.Length && memory[startingPointer] != -1)
                startingPointer++;

            //move ending pointer to the next none -1
            while (endingPointer > 0 && memory[endingPointer] == -1)
                endingPointer--;

            //if we reached the end of the array or the starting pointer is greater than the ending pointer
            if (startingPointer == memory.Length || endingPointer == 0 || startingPointer >= endingPointer)
                break;

            swap = memory[startingPointer];
            memory[startingPointer] = memory[endingPointer];
            memory[endingPointer] = swap;
        }

        var result = CalculateChecksum(memory);
        _logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }

    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 9);

        //split data every character
        var data = input
            .SelectMany(x => x.ToString())
            .Select(x => int.Parse(x.ToString()))
            .ToList();

        var memory = PreAllocateMemory(data);

        //find all empty memory slots, ie contiguous -1
        var emptyMemorySlots = new List<(int startIndex, int slotSize)>();
        for (var memoryIndex = 0; memoryIndex < memory.Length; memoryIndex++)
        {
            if (memory[memoryIndex] != -1)
                continue;

            //find the size of the empty memory slot
            var size = 1;
            while (memoryIndex + size < memory.Length && memory[memoryIndex + size] == -1)
                size++;

            emptyMemorySlots.Add((memoryIndex, size));
            memoryIndex += size;
        }

        var endingPointer = memory.Length - 1;
        while (true)
        {
            //move ending pointer to the next none -1
            while (endingPointer > 0 && memory[endingPointer] == -1)
                endingPointer--;

            //did we reached the end of the memory?
            if (endingPointer <= 0)
                break;

            //find the occurences of the current number at the pointer
            var current = memory[endingPointer];
            var occurences = 0;
            while (endingPointer > 0 && memory[endingPointer] == current)
            {
                occurences++;
                endingPointer--;
            }

            //find the first empty memory slot that can hold the occurences
            var emptyMemorySlot = emptyMemorySlots.FirstOrDefault(x => x.slotSize >= occurences && x.startIndex <= endingPointer);
            if (emptyMemorySlot == default)
                continue;

            //swap the occurences with the empty memory slot
            for (var i = 0; i < occurences; i++)
            {
                memory[emptyMemorySlot.startIndex + i] = current;
                memory[endingPointer + i + 1] = -1;
            }

            //update the empty memory slot
            var emptyMemorySlotIndex = emptyMemorySlots.IndexOf(emptyMemorySlot);
            emptyMemorySlot = (emptyMemorySlot.startIndex + occurences, emptyMemorySlot.slotSize - occurences);
            emptyMemorySlots[emptyMemorySlotIndex] = emptyMemorySlot;
        }

        var result = CalculateChecksum(memory);
        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}

