using MedCon.Models;
using MedCon.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.ViewModels
{
   public class NotificationsViewModel:ViewModelBase
    {
       public ObservableCollection<Notification> notifications { get; set; }
        public NotificationsViewModel()
        {
            notifications = new ObservableCollection<Notification>();
            notifications.Add(new Notification { NotificationText="You have a new Titrate for trail one day.Please test it once",Time="2 min ago"});
            notifications.Add(new Notification { NotificationText = "You will be part of new trail starting from next week. Hope you test it", Time = "10 min ago" });

        }
    }

}
