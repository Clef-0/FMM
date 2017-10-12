using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using IniParser;
using IniParser.Model;
using WPF.JoshSmith.ServiceProviders.UI;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace FMM2
{
    public partial class MainWindow : Window
    {
        private void listViewHeaderClicked<T>(ListView list, object colOC, RoutedEventArgs e) where T : CollectionItem
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;
            

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (true)
                    {
                        if (headerClicked != _lastHeaderClicked)
                        {
                            direction = ListSortDirection.Ascending;
                        }
                        else
                        {
                            if (_lastDirection == ListSortDirection.Ascending)
                            {
                                direction = ListSortDirection.Descending;
                            }
                            else
                            {
                                direction = ListSortDirection.Ascending;
                            }
                        }

                        string header = headerClicked.Column.Header as string;

                        listViewSort<T>(list, colOC, header, direction);

                        _lastHeaderClicked = headerClicked;
                        _lastDirection = direction;
                    }
                }
            }
        }

        private bool returnTrue(object x)
        {
            return true;
        }

        private void listViewSort<T>(ListView list, object colOC, string sortBy, ListSortDirection direction) where T : CollectionItem
        {
            if (sortBy != null)
            {
                ICollectionView dataView = CollectionViewSource.GetDefaultView(list.ItemsSource);
                Predicate<object> savedPredicate = dataView.Filter;
                dataView.Filter = new Predicate<object>(returnTrue);
                List<T> checkedItems = new List<T>();

                foreach (T item in dataView)
                {
                    if (item.IsChecked == true)
                    {
                        checkedItems.Add(item);
                    }
                }

                dataView.SortDescriptions.Clear();
                SortDescription sd = new SortDescription(sortBy, direction);
                dataView.SortDescriptions.Add(sd);
                dataView.Refresh();

                ObservableCollection<T> newCol = new ObservableCollection<T>();
                foreach (T item in dataView)
                {
                    newCol.Add(item);
                }

                ((ObservableCollection<T>)colOC).Clear();

                foreach (T item in newCol)
                {
                    ((ObservableCollection<T>)colOC).Add(item);
                }

                checkedItems.Reverse();
                foreach (T item in checkedItems)
                {
                    T itemToMoveUp = (((ObservableCollection<T>)colOC)[((ObservableCollection<T>)colOC).IndexOf(item)]);
                    ((ObservableCollection<T>)colOC).RemoveAt(((ObservableCollection<T>)colOC).IndexOf(item));
                    ((ObservableCollection<T>)colOC).Insert(0, itemToMoveUp);
                }
                dataView.SortDescriptions.Clear();
                dataView.Filter = savedPredicate;
            }
            else
            {
                bool uncheck = true;
                foreach (T item in ((ObservableCollection<T>)colOC))
                {
                    if (item.IsChecked == false)
                    {
                        uncheck = false;
                    }
                    
                }

                foreach (T item in ((ObservableCollection<T>)colOC))
                {
                    //added logic to only check shown items
                    if (list.Items.Contains(item)) { 
                        if (uncheck)
                            item.IsChecked = false;
                        else
                            item.IsChecked = true;
                    }
                }
            }
        }
    }
}
