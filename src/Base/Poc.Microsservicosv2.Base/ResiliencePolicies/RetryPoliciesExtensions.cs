using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace Poc.Microsservicosv2.Base.ResiliencePolicies;

public static partial class RetryPoliciesExtensions
{
    public static IHttpClientBuilder AddHttpResponseWaitAndRetryPolicyHandler(
        this IHttpClientBuilder builder,
        int retryCount = 2,
        int secondsOfWaiting = 1)
    {
        return builder.AddPolicyHandler(
            HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: retryCount,
                sleepDurationProvider: duration => TimeSpan.FromSeconds(Math.Pow(secondsOfWaiting, duration))));
    }

    public static IServiceCollection AddSingletonWithRetry<TService, TKnowException>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> implementationFactory,
        int retryCount)
        where TKnowException : Exception
        where TService : class
    {
        return services.AddSingleton(serviceProvider =>
        {
            TService returnValue = default;

            BuildPolicy<TKnowException>(retryCount: retryCount).Execute(() =>
            {
                returnValue = implementationFactory(serviceProvider);
            });

            return returnValue;

        });
    }

    public static IServiceCollection AddTransientWithRetry<TService, TKnowException>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> implementationFactory,
        int retryCount)
        where TKnowException : Exception
        where TService : class
    {
        return services.AddTransient(serviceProvider =>
        {
            TService returnValue = default;

            BuildPolicy<TKnowException>(retryCount: retryCount).Execute(() =>
            {
                returnValue = implementationFactory(serviceProvider);
            });

            return returnValue;

        });
    }


    //
    private static RetryPolicy BuildPolicy<TKnowException>(int retryCount) where TKnowException : Exception
    {
        return Policy
            .Handle<TKnowException>()
            .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
        );
    }
}
