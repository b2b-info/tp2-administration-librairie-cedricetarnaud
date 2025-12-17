using BookStore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;


    internal class ProducerConsumerPatternHandler
    {
        public static readonly Channel<Operations> TasksQueue = Channel.CreateUnbounded<Operations>();

        public static ValueTask Produce(Operations operation, string actionQueud)
        {
            Program.logger.LogInformation(actionQueud);
            return TasksQueue.Writer.WriteAsync(operation);
        }
        public static async Task Consume(CancellationToken cancellationToken)
        {
            await foreach (var operation in TasksQueue.Reader.ReadAllAsync(cancellationToken))
            {
                operation.ExecuteState();
                await Task.Delay(100, cancellationToken);
            }
        }
    }

