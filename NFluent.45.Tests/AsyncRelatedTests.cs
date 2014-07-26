﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="AsyncRelatedTests.cs" company="">
// //   Copyright 2014 Thomas PIERRAIN
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------
namespace NFluent.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using NUnit.Framework;

    [TestFixture]
    public class AsyncRelatedTests
    {
        [Test]
        public void CheckThatCodeOnFinishedAsyncMethodReturnsAggregateExceptionInsteadOfTheOriginalExceptionType()
        {
            // bad way for async methods
            Check.ThatCode(this.DoSomethingBadAsync().Wait).Throws<AggregateException>();
        }

        [Test]
        public void CheckThatAsyncCodeOnAsyncMethodReturnsTheOriginalExceptionType()
        {
            // proper way for async methods
            Check.ThatAsyncCode(this.DoSomethingBadAsync).Throws<InvalidOperationException>();
        }

        [Test]
        public void CheckThatAsyncCodeOnAsyncFunctionReturnsTheOriginalExceptionType()
        {
            // proper way for async methods
            Check.ThatAsyncCode((AwaitableFunction<int>)this.DoSomethingBadBeforeTheAnswerAsync).Throws<InvalidOperationException>();
        }

        [Test]
        public void CheckThatAsyncCodeWorksForFunctions()
        {
            // proper way for async methods
            Check.ThatAsyncCode((AwaitableFunction<int>)this.ThinkAndReturnTheAnswerAsync).DoesNotThrow().And.WhichResult().IsEqualTo(42);
        }

        private async Task DoSomethingBadAsync()
        {
            await Task.Run(() =>
            {
                // This operation takes a while
                Thread.Sleep(100);
                throw new InvalidOperationException("What da heck?!?");
            });
        }

        private async Task<int> ThinkAndReturnTheAnswerAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(100);
            });

            return 42;
        }

        private async Task<int> DoSomethingBadBeforeTheAnswerAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(100);
                throw new InvalidOperationException("Too bad mate!");
            });

            return 42;
        }
    }
}
