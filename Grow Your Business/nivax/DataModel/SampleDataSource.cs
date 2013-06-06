using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Ideas & Directions",
                    "Ideas & Directions",
                    "Assets/Images/10.jpg",
                    "To run your business at its very best, use our step-by-step process to help you tweak, streamline and turbo-charge the performance and profitability of your business.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Measure and Analyze Your Current Status",
                    "It’s time to take stock. At the beginning of our 10 Steps to Grow Your Business, we start by walking you through a reality check. We’ll help you build an accurate and complete understanding of your business’ current status. So armed with these tools to build your business, you can then formulate strategies to realize your vision of success.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nIt’s time to take stock. At the beginning of our 10 Steps to Grow Your Business, we start by walking you through a reality check. We’ll help you build an accurate and complete understanding of your business’ current status. So armed with these tools to build your business, you can then formulate strategies to realize your vision of success. HERE ARE THE FIRST STEPS TO TAKE: \n\nEstablish Key Metrics Understand Your Financials - Review Margins, Cash Flow Review the Competitive Landscape Consider Customer Surveys Use Online Analytics Revisit Assumptions from Your Business Plan Establish Key Metrics\n\nThe very first thing to do before plotting a course for growth is to set up the key metrics of your business. “Metrics” are basically crucial statistics by which you can measure how well your business is performing. Among the key metrics to consider are money-related metrics, customer-related metrics, product development-related metrics and team- and operations-related metrics. For example, a customer-related metric might be, “Conversion rate of Web site visitors to purchasers.” Is 5 percent satisfactory or do you need a higher number like 15 percent to generate the revenue you’re seeking? Knowing this metric may enable you to make adjustments to your online strategy that could be very powerful to your business.\n\nA financial metric might be something like, “Monthly cash flow.” You may need to achieve and maintain monthly profitability by end of Q2, for example, and if you don’t, you may have to reduce your spending until the situation is rectified. \n\nOr how about an operational metric, focusing on inventory turns? This is all about how quickly you move product out the door. Knowing this will help you understand how long you can expect your money to be sunk in merchandise instead of being available for other business activities, like meeting payroll. Critical aspects of creating key metrics include: Choosing those things that have a significant ripple effect on the performance of your business Ensuring that the metrics are measured over time. Having quantifiable measurability so you can be sure you understand whether or not you’re performing.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Ideas & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Measure and Analyze Your Current Status", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Grow Your Business" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Get Efficient through Technology",
                     "Working hard is important. But there’s one thing even more important - working smart. That’s what technology helps you do. You get more done, you get it done easier and you get it done better.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nWorking hard is important. But there’s one thing even more important - working smart. That’s what technology helps you do. You get more done, you get it done easier and you get it done better. Frankly, by using technology you’re able to do some things that would simply be impossible without it. Luckily, there’s an array of tech tools that allow you to build your business in extremely smart ways. Be sure to take full advantage of them. IN THIS STEP, WE WILL LOOK AT FOUR CATEGORIES OF TECHNOLOGY PRODUCTS: Computers Gadgets Software Online Tools First, you have to know what’s out there. Second, you need to understand all the cool things that technology can do for you. After all, what’s the point of buying an expensive gadget if you can’t revel in all of its neat tricks?\n\nSelecting a computer may seem straightforward, but you should carefully consider the way you do business before making your next computer investment. Let’s say you’re an on-the-go real estate agent who needs data at your fingertips. Your best bet is probably a laptop. Your work goes everywhere with you, whether it’s to a client’s office or the coffee shop around the corner. But if you spend most of your time at the office and rarely emerge, you’re better suited to a desktop computer. Most of all, a desktop forces you to designate a special place for your work, which helps you fully focus on the task at hand. Are you using the computer primarily for graphic design or writing and numbers-crunching? Graphic designers and people who produce Web sites or newsletters sometimes prefer Macs, though the gap between PCs and Macs has nearly disappeared. Whether you’re more of a laptop or desktop person, don’t forget one of the most important computer accessories of all – the monitor. Since you’re likely to spend extended periods of time in front of it, be sure you don’t skimp in either size or quality. Consider a flat screen monitor – it saves room on your desk and comes in sizes small, medium and large.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ideas & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Get Efficient through Technology", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Grow Your Business" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                     "Enhance Your Customer's Experience",
                     "Formalize a Customer Relationship Management (CRM) Strategy Coddle them, cater to them, treat them like they’re the most important asset of your business, and your customers will grow your business for you.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nFormalize a Customer Relationship Management (CRM) Strategy Coddle them, cater to them, treat them like they’re the most important asset of your business, and your customers will grow your business for you. Rather then being informal and ad hoc about customers, it’s very important to create a well-thought-out system for managing the relationships you have – or want to have – with them. In business circles, this is called “customer relationship management” or CRM.First, before you try to formalize a CRM plan, find the weak links in your service chain. To do this, you may want to revisit our advice on using surveys in Step 1 of 10 Steps to Grow Your Business. Surveys, whether online or in person, are a great way to collect feedback from your current customers. Also consider selecting a subset of survey respondents or simply a handful of loyal customers and invite them to act as advisors to your business. Why would people do either? Well, it may be because they have an opinion they’d like to share, and this is a great way to be heard. Or, it may be that they’d find it gratifying and stimulating to help you succeed. There could be other reasons, but there’s nothing like letting your customers lead you to improved offerings.\n\nIf your primary contact with customers is by phone, an unimposing way to collect information about their experience is to ask for feedback right at the time of the transaction or whatever business activity you’re conducting. Ask what you could have done better or what other offerings would interest them. There are technology solutions designed to help you note, classify, track, respond, call back and bill customers, among other functions: CRM software. The beauty of these systems, made by a number of leading vendors, is that you become supremely organized, prioritized and productive when your customer interactions are driven by software. Not to say you’re sloppy or disorganized, but if you’re going to grow, you have to streamline and upgrade your methods of handling cherished customers. CRM software packages can widely range in price depending on complexity and functionality, but you can be sure there are affordable solutions for even the smallest businesses.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ideas & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Enhance Your Customer's Experience", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Grow Your Business" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "Cozy Up with Vendors",
                     "You hate not getting paid on time, right? The challenge is to maintain a healthy inventory supply, which creates revenue-generating opportunities, while you’re operating in a typical environment where inbound revenues are slow.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nYou hate not getting paid on time, right? The challenge is to maintain a healthy inventory supply, which creates revenue-generating opportunities, while you’re operating in a typical environment where inbound revenues are slow. This is a cash-flow challenge and can significantly hamper your ability to run your business strategically. The trick is to work closely with vendors to negotiate mutually agreeable payment terms. If they want your business badly enough, they may agree to ease up on the timing of your payments. For example, their current terms might be pay-on-delivery; they might be the typical net-30. But with the right arm-twisting — promises to focus on them for your buying, or other incentives — you may well be able to establish such extended payment terms as net-45 or 60. If their cash-flow constraints aren’t as sensitive as yours, it can be done.What’s this mean to you? It means you can use revenue from operations, rather than more expensive forms of financing, and this means a better bottom line. Shortening payment timeframes Alternately, you might want to do the opposite – pay your vendors early. Does this make sense? Only if they give you a financial incentive. If cash-flow sensitivity is not too bad with your business, you might be able to arrange a percentage discount on payables. The same applies to office equipment and services. Here’s how it works: Instead of paying on the traditional net 30-day terms, ask for a 2 percent discount if you pay within 10 days. For vendors who are cash-flow vulnerable, it may be very appealing. For you, it means a slight discount on every order. And every couple of percentage points you save here add to your profit margins. Using basic math, let’s say your business has gross profit margins of 20 percent, and you take advantage of the “Net 10 / 2%” payment terms on a consistent basis. This would result in a 2 percent discount on inventory costs, and would increase your profit margin from 20 percent to 22 percent. That’s a 10 percent improvement in total profit margin, and that’s real money you keep for very little effort.\n\nReducing exposure\nIf you’re in retail, this recommendation is particularly worth considering: Transfer inventory risk to your vendors almost completely. It can be pulled off either by encouraging consignment terms, where you never buy the product, but simply offer it and earn a commission on sales – or by implementing a “guaranteed sale” policy, where vendors must buy back any product that doesn’t sell. This is extreme, but often newer vendors will go along in an effort to prove themselves and their unproven products. This applies whether you’re an online retailer or a traditional brick-and-mortar business.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ideas & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Cozy Up with Vendors", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Grow Your Business" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                     "Maximize Your Niche, Expand to a New One",
                     "Expand What You Offer Once you’re on your way with your initial product or service, you can expand by adding complementary offerings to your current ones. Be sure you have crisp and credible answers to the following questions before you decide which products or services to add.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nExpand What You Offer Once you’re on your way with your initial product or service, you can expand by adding complementary offerings to your current ones. Be sure you have crisp and credible answers to the following questions before you decide which products or services to add: What can I introduce to leverage my existing customer base? Your current customers are the least expensive (most profitable) to “cross sell.” What can I introduce to leverage my existing business resources? It may be easier to expand your offering if you can figure out something that takes advantage of current facilities, Web site, salespeople, etc. What can I introduce that’s a “natural” addition based on what I already provide?Are there things that clearly go well with what you’re already offering?\n\nWhat can I introduce that requires the least capital investment or financial risk? Avoid big capital outlays if possible – this is all about making more money, not getting deeper into a financial hole! How do I get early feedback so I know whether or not it’s working? Swarm the first customers to learn whether your new offering is being embraced the way you hoped it would.\n\nEnter a New Niche \nWhen you feel you really have command of your current market niche, you may want to look for new opportunities in markets you’re currently not addressing.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ideas & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Maximize Your Niche, Expand to a New One", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Grow Your Business" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                     "Develop New Channels",
                     "Tap the Web Think about what’s happened during the past decade alone. Let’s look at the real estate industry as an example. Ten years ago, about 10 percent of the house-buying public was going online to find new homes. Today, 77 percent handle their searches online, according to the National Association of Realtors.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nTap the Web Think about what’s happened during the past decade alone. Let’s look at the real estate industry as an example. Ten years ago, about 10 percent of the house-buying public was going online to find new homes. Today, 77 percent handle their searches online, according to the National Association of Realtors. Or, consider that 70 percent of adults use the Web. And you should use it to reach them. If you’re not online now, regardless of your business type, chances are you’re missing valuable opportunities for additional business. In an age of short attention spans and a growing need for instant gratification, Web surfing is more appealing than ever to business and retail consumers. Here are things you can achieve by tapping the Internet: Messaging You can establish your brand image and express key features or aspects of what you offer. Transaction. You can sell your products through the Internet. Satisfy a consumer’s impulse On the Internet you can sell at “the speed of thought.” In essence, as quickly as someone has an urge to buy, you can sell it to them. Compare that to a brick-and-mortar store that’s open from 9 to 5 and only in one town. Deliver collateral material Instead of printing fancy brochures that are static and require delivery, you can electronically create a PDF file of your brochure for people to instantly.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ideas & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Develop New Channels", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/16.jpg")), CurrentStatus = "Grow Your Business" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                     "Acquire Growth Capital",
                     "Vendor and Customer Financing The least expensive way to finance growth is to work with your vendors and customers. If you‘re established and have a great supplier base, go to them and explain your growth plan and various initiatives. Ask them to work with you on extended payment terms.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nVendor and Customer Financing The least expensive way to finance growth is to work with your vendors and customers. If you‘re established and have a great supplier base, go to them and explain your growth plan and various initiatives. Ask them to work with you on extended payment terms. If you have 30-days, get them to stretch it to 60, 75 or even 90 for a defined period of time. Once you get through, use the experience to look for other forms of financing. Your cooperative suppliers will be rewarded with a bigger customer (you) and more timely payment terms. If you have long-standing customers who depend on you, ask them for a large purchase order that can be factored (see below), or to accept early delivery of goods, or to pre-pay for a large order. If the value you propose in return is meaningful and hard to replace, it could be the best and least expensive form of financing there is. Upside \nYou maintain complete financial and operational control over your business. No equity-holders to pay off if the company hits it big. You grow your operations with new and/or existing customer and vendor relationships that will only get stronger. Downside Typically, this form of funding limits the amount of money you have for strategic purposes, so your business growth can be slowed way down as it starves for cash. If you miss delivery to a customer or overextend with vendors, you may damage business relationships that you can’t afford to lose if you hope to recover.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ideas & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Acquire Growth Capital", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/17.jpg")), CurrentStatus = "Grow Your Business" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                     "Create a Culture",
                     "Some growth strategies are obvious and have immediate and tangible results. Others, like creating a culture, have more indirect results, but are still extremely important as you strive to make your organization more dynamic, more productive and more profitable.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nSome growth strategies are obvious and have immediate and tangible results. Others, like creating a culture, have more indirect results, but are still extremely important as you strive to make your organization more dynamic, more productive and more profitable. Creating a culture for your company is about cultivating passion in both your team and customer base. Your culture clarifies your identity, your values and your beliefs, in addition to more basic things like the products or services you offer and how you price them. Not sure what we mean by “culture”? Think “Enron,” and you generate a slew of images and adjectives of a company culture. To the other extreme, mention “Disney,” “Starbucks” or “ Patagonia,” and the thought process goes in opposite directions. When you create a company culture, you’ll find yourself and your employees all dancing around the same bonfire. It unifies your mission and adds meaning (and fun) to your daily activities. When this culture permeates all you do, you’ll find your customers picking up on it and responding to it. The kind of loyalty it can bring about can lead to significant repeat business and positive word-of-mouth – both invaluable. To accurately convey who you are and what your business is all about, we provide some helpful tasks for you to complete.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ideas & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Create a Culture", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/18.jpg")), CurrentStatus = "Grow Your Business" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                     "Ramp Up Awareness and Demand",
                     "Catching people’s attention isn’t easy these days. To do it, you have to make some noise, create some sizzle and be compelling. Special promotions do exactly that. First and foremost, you have to decide on your objectives.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nCatching people’s attention isn’t easy these days. To do it, you have to make some noise, create some sizzle and be compelling. Special promotions do exactly that. First and foremost, you have to decide on your objectives. For example, is your goal to create better branding and awareness for your business? Or is it the more direct objective of actually generating sales leads? Is it more focused on new customers or current ones? From this fundamental level, you can start conjuring up your promotional concepts. You’ll have to identify something that’s consistent with your company’s brand and attractive to your type of customer. Believe it or not, your customer needn’t know exactly what your goal is; you may want to educate them about a new offer without them knowing that’s the purpose of the promotion.\n\nIt’s conceivable that people “enter to win” a reward that has nothing to do with what you sell – but in the process you can share information with them about what you do and why it’s potentially valuable to them. And it’s also conceivable that you wish to be blunter about your promotion. If you sell cars, want more buyers and want your promotion to get people onto your lot, a car-lease giveaway may be just the thing. You should factor into this decision the actual delivery method. You might decide the Internet is the best way to get the promotion in front of your desired audience. Or you might choose to place sign-up sheets at your social club or to display your promotion next to the cash registers in local stores. It’s up to you to decide how best to reach your target given their preferences, your objectives and your budget.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ideas & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Ramp Up Awareness and Demand", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/19.jpg")), CurrentStatus = "Grow Your Business" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                     "Improve Sales Techniques",
                     "The first and most important thing you can do to upgrade your sales results is very simple: Make your sales effort a priority. It seems obvious, but sometimes with all the minutiae of running a business, you get caught up in other activities that don’t put you directly on the path to growth.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThe first and most important thing you can do to upgrade your sales results is very simple: Make your sales effort a priority. It seems obvious, but sometimes with all the minutiae of running a business, you get caught up in other activities that don’t put you directly on the path to growth. Over time, this can become a rhythm that sticks you in the status quo, stagnating. If this sounds familiar to you, it’s time to change your mindset. To knock the dust off your current sales effort, here are some easy but helpful tips to move you into proactive selling mode. Create a daily sales checklist. For example, identify the number of accounts you’ll approach today. \n\nSet aside a specific amount of time each day when you’re focused strictly on selling activity (or at least, someone at your business is). Treat your selling time as sacred. Don’t let inbound e-mails or piles of snail mail distract you or break your focus. To sell effectively, you have to be in selling mode. Know your product or service in and out, rehearse your script, get upbeat, heck — listen to a favorite inspiring song if that’s what it takes! Just be sure you get in the zone, because this kind of energy brings out the best in you. Plus, it’s wildly contagious. Look at sales as a numbers game. Rejection is an opportunity to learn, streamline your pitch and try again. Be sure you crank up the pipeline of potential sales so you have good odds of closing on enough business to meet your goals. Set performance goals. For example, you will achieve $X in sales during Y period of time. If you don’t reach the goals, you need to analyze why. It could be that you need to, a) make changes in what you’re offering, b) change the way you’re offering it, or perhaps, C) reset your goals at more reasonable levels. Unify everyone’s effort at the company by sharing the goals team-wide. There’s nothing like keeping everyone informed and aligned to create an environment where people are armed to be proactive.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ideas & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Improve Sales Techniques", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/20.jpg")), CurrentStatus = "Grow Your Business" });
					 
            this.AllGroups.Add(group1);


			
			
         
        }
    }
}
