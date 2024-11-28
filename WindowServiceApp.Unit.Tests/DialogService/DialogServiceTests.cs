
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WpfApp1;
using WpfApp1.ViewModels;
using WpfApp1.Views;
using Xunit;

namespace WindowServiceApp.Unit.Tests.DialogService;


[Collection("STA")]
public class DialogServiceTests
{
    private readonly IServiceProvider serviceProvider;
    private readonly WpfApp1.DialogService dialogService;

    public DialogServiceTests()
    {
        serviceProvider = Substitute.For<IServiceProvider>();
        WpfApp1.DialogService.ServiceProviderFactory = serviceProvider;
        dialogService = new WpfApp1.DialogService();
    }

    [Fact]
    public void ShowDialog_WithResult_ShouldInvokeCallback()
    {
        // Arrange
        WpfApp1.DialogService.RegisterDialog<ChildView, TestViewModel>();
        var callbackInvoked = false;
        Func<string, TestViewModel, bool> callback = (result, vm) => callbackInvoked = true;
        
        var parameters = new { Param1 = "Value1" };
        // Act
        var thread = new Thread(() =>
            {
                var dialogService = new WpfApp1.DialogService();
                dialogService.ShowDialog<TestViewModel, string>(callback, parameters);
            });
        
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        
        // Assert
        Assert.True(callbackInvoked);
    }

    [Fact]
    public void RegisterDialog_ShouldAddMapping()
    {
        // Arrange
        WpfApp1.DialogService.RegisterDialog<ChildView, TestViewModel>();
    
        // Act
        var mappings = typeof(WpfApp1.DialogService).GetField("Mappings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.GetValue(null) as Dictionary<Type, Type>;
    
        // Assert
        Assert.NotNull(mappings);
        Assert.True(mappings.ContainsKey(typeof(TestViewModel)));
        Assert.Equal(typeof(ChildView), mappings[typeof(TestViewModel)]);
    }

    [Fact]
    public void ShowDialog_ByName_ShouldInvokeCallback()
    {
        WpfApp1.DialogService.RegisterDialog<ChildView, ChildViewModel>();
        serviceProvider.GetService<ChildViewModel>().Returns(new ChildViewModel());

        var thread = new Thread(() =>
        {
            // Arrange
            var callbackInvoked = false;
            Action<bool> callback = result => callbackInvoked = true;

            // Act
            dialogService.ShowDialog<ChildViewModel>(callback);

            // Assert
            Assert.True(callbackInvoked);
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
    }

    [Fact]
    public void ShowDialog_ByViewModel_ShouldInvokeCallback()
    {
        // Arrange
        WpfApp1.DialogService.RegisterDialog<ChildView, TestViewModel>();
        var callbackInvoked = false;
        Action<bool> callback = result => callbackInvoked = true;

        // Act
        dialogService.ShowDialog<TestViewModel>(callback);

        // Assert
        Assert.True(callbackInvoked);
    }

    [Fact]
    public void ShowDialog_WithParameters_ShouldInvokeCallback()
    {
        // Arrange
        WpfApp1.DialogService.RegisterDialog<ChildView, TestViewModel>();
        var callbackInvoked = false;
        Action<bool> callback = result => callbackInvoked = true;
        var parameters = new { Param1 = "Value1" };

        // Act
        dialogService.ShowDialog<TestViewModel>(callback, parameters);

        // Assert
        Assert.True(callbackInvoked);
    }

    // [Fact]
    // public void ShowDialog_WithResult_ShouldInvokeCallback()
    // {
    //     // Arrange
    //     WpfApp1.DialogService.RegisterDialog<DialogWindow, TestViewModel>();
    //     var callbackInvoked = false;
    //     Func<string, TestViewModel, bool> callback = (result, vm) => callbackInvoked = true;
    //     var parameters = new { Param1 = "Value1" };
    //
    //     // Act
    //     dialogService.ShowDialog<TestViewModel, string>(callback, parameters);
    //
    //     // Assert
    //     Assert.True(callbackInvoked);
    // }

    private class TestViewModel
    {
        public string DialogResult { get; set; } = "TestResult";
    }
}


[CollectionDefinition("STA")]
public class STACollection : ICollectionFixture<STAFixture>
{
}

public class STAFixture
{
    public STAFixture()
    {
        var staThread = new Thread(() => { });
        staThread.SetApartmentState(ApartmentState.STA);
        staThread.Start();
        staThread.Join();
    }
}



