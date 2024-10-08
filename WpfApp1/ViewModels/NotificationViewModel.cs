﻿using Core;

namespace WpfApp1.ViewModels;

public class NotificationViewModel: BindableBase
{
    private readonly ITimeService timeService;
    private string currentTime;
    private readonly CustomDynamic dynamic;

    public bool DialogResult { get; set; }

    public CpiSkuDimensionVariant SelectedCpiSkuDimensionVariant { get; set; }
    public string SelectedSku { get; set; }
    
    public string CurrentTime
    {
        get => currentTime;
        set => SetProperty(ref currentTime, value);
    }

    // public NotificationViewModel(ITimeService timeService)
    // {
    //     this.timeService = timeService;
    //     CurrentTime = timeService.GetTime().ToString();
    //     
    //     dynamic = new CustomDynamic
    //         {
    //             ["FirstName"] = "John",
    //             ["LastName"] = "Doe"
    //         };
    //     
    //     CurrentTime = dynamic.ToString();
    // }
  
}