﻿// Copyright (c) Richasy. All rights reserved.

using System;
using Bilibili.App.Card.V1;
using Bilibili.App.Show.V1;
using Bilibili.App.View.V1;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频视图模型.
    /// </summary>
    public partial class VideoViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="video">分区视频.</param>
        public VideoViewModel(PartitionVideo video)
            : this()
        {
            Title = video.Title ?? string.Empty;
            Publisher = new UserViewModel(video.Publisher, video.PublisherAvatar);
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(video.Duration));
            PlayCount = _numberToolkit.GetCountText(video.PlayCount);
            ReplyCount = _numberToolkit.GetCountText(video.ReplyCount);
            DanmakuCount = _numberToolkit.GetCountText(video.DanmakuCount);
            LikeCount = _numberToolkit.GetCountText(video.LikeCount);
            VideoId = video.Parameter;
            PartitionName = video.PartitionName;
            PartitionId = video.PartitionId;
            Source = video;
            LimitCover(video.Cover);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="video">排行榜视频.</param>
        public VideoViewModel(Item video)
            : this()
        {
            Title = video.Title ?? string.Empty;
            Publisher = new UserViewModel(video.Name, video.Face, Convert.ToInt32(video.Mid));
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(video.Duration));
            PlayCount = _numberToolkit.GetCountText(video.Play);
            ReplyCount = _numberToolkit.GetCountText(video.Reply);
            DanmakuCount = _numberToolkit.GetCountText(video.Danmaku);
            LikeCount = _numberToolkit.GetCountText(video.Like);
            VideoId = video.Param;
            PartitionName = video.Rname;
            Source = video;
            PartitionId = video.Rid;
            AdditionalText = video.Pts.ToString();
            LimitCover(video.Cover);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="card">推荐视频卡片.</param>
        public VideoViewModel(RecommendCard card)
            : this()
        {
            Title = card.Title ?? string.Empty;
            VideoId = card.Parameter;
            PlayCount = card.PlayCountText;
            if (card.CardGoto == ServiceConstants.Av)
            {
                // 视频处理.
                DanmakuCount = card.SubStatusText;
                LikeCount = string.Empty;
                Publisher = new UserViewModel(card.CardArgs.PublisherName, userId: card.CardArgs.PublisherId);
                if ((card.PlayerArgs?.Duration).HasValue)
                {
                    Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds((double)card.PlayerArgs?.Duration));
                }
                else
                {
                    Duration = _numberToolkit.FormatDurationText(card.DurationText);
                }

                PartitionId = card.CardArgs.PartitionId;
                PartitionName = card.CardArgs.PartitionName;
            }
            else
            {
                // 动漫处理.
                LikeCount = card.SubStatusText;
                DanmakuCount = string.Empty;
                Publisher = new UserViewModel(card.Description?.Text);
                Duration = "--";
                VideoType = VideoType.Pgc;
            }

            AdditionalText = card.RecommendReason ?? string.Empty;
            Source = card;
            LimitCover(card.Cover);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="card">视频卡片.</param>
        public VideoViewModel(Card card)
            : this()
        {
            var v5 = card.SmallCoverV5;
            var cardBase = v5.Base;
            Title = cardBase.Title;
            VideoId = cardBase.Param;
            PlayCount = v5.RightDesc2;
            Publisher = new UserViewModel(v5.RightDesc1);
            AdditionalText = v5.RcmdReasonStyle?.Text ?? string.Empty;
            Duration = _numberToolkit.FormatDurationText(v5.CoverRightText1);
            LimitCover(cardBase.Cover);
            Source = card;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="followRoom">关注的直播间.</param>
        public VideoViewModel(LiveFeedFollowRoom followRoom)
            : this()
        {
            Title = followRoom.Title;
            VideoId = followRoom.RoomId.ToString();
            ViewerCount = _numberToolkit.GetCountText(followRoom.ViewerCount);
            Publisher = new UserViewModel(followRoom.UserName, followRoom.UserAvatar, followRoom.UserId);
            PartitionName = followRoom.DisplayAreaName;
            PartitionId = Convert.ToInt32(followRoom.DisplayAreaId);
            LimitCover(followRoom.Cover);
            Source = followRoom;
            VideoType = VideoType.Live;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="card">直播间卡片.</param>
        public VideoViewModel(LiveRoomCard card)
            : this()
        {
            Title = card.Title;
            VideoId = card.RoomId.ToString();
            ViewerCount = card.CoverRightContent.Text;
            Publisher = new UserViewModel(card.CoverLeftContent.Text);
            PartitionName = card.AreaName;
            PartitionId = Convert.ToInt32(card.AreaId);
            LimitCover(card.Cover);
            Source = card;
            VideoType = VideoType.Live;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="relate">相关视频推荐.</param>
        public VideoViewModel(Relate relate)
            : this()
        {
            Title = relate.Title;
            VideoId = relate.Aid.ToString();
            PlayCount = _numberToolkit.GetCountText(relate.Stat.View);
            DanmakuCount = _numberToolkit.GetCountText(relate.Stat.Danmaku);
            LikeCount = _numberToolkit.GetCountText(relate.Stat.Like);
            ReplyCount = _numberToolkit.GetCountText(relate.Stat.Reply);
            var author = relate.Author;
            Publisher = new UserViewModel(author.Name, author.Face, Convert.ToInt32(author.Mid));
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(relate.Duration));
            AdditionalText = relate.Rating.ToString();
            LimitCover(relate.Pic);
            Source = relate;
            VideoType = relate.Goto.Equals(ServiceConstants.Av, StringComparison.OrdinalIgnoreCase) ?
                VideoType.Video : VideoType.Pgc;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="item">视频搜索结果.</param>
        public VideoViewModel(VideoSearchItem item)
            : this()
        {
            VideoType = VideoType.Video;
            Title = item.Title;
            VideoId = item.Parameter;
            PlayCount = _numberToolkit.GetCountText(item.PlayCount);
            DanmakuCount = _numberToolkit.GetCountText(item.DanmakuCount);
            Publisher = new UserViewModel(item.Author, item.Avatar, item.UserId);
            Duration = _numberToolkit.FormatDurationText(item.Duration);
            LimitCover(item.Cover);
            Source = item;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="item">直播搜索结果.</param>
        public VideoViewModel(LiveSearchItem item)
            : this()
        {
            VideoType = VideoType.Live;
            Title = item.Title;
            VideoId = item.RoomId.ToString();
            Publisher = new UserViewModel(item.Name, userId: item.UserId);
            ViewerCount = _numberToolkit.GetCountText(item.ViewerCount);
            PartitionName = item.AreaName;
            LimitCover(item.Cover);
            Source = item;
            VideoType = VideoType.Live;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="item">用户空间视频条目.</param>
        public VideoViewModel(UserSpaceVideoItem item)
            : this()
        {
            VideoType = item.IsPgc ? VideoType.Pgc : VideoType.Video;
            Title = item.Title;
            VideoId = item.Id;
            PartitionName = item.PartitionName;
            PlayCount = _numberToolkit.GetCountText(item.PlayCount);
            DanmakuCount = _numberToolkit.GetCountText(item.DanmakuCount);
            Publisher = new UserViewModel(item.PublisherName);
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(item.Duration));
            LimitCover(item.Cover);
            Source = item;
        }

        internal VideoViewModel()
        {
            ServiceLocator.Instance.LoadService(out _numberToolkit);
            VideoType = VideoType.Video;
        }

        /// <summary>
        /// 限制图片分辨率以减轻UI和内存压力.
        /// </summary>
        private void LimitCover(string coverUrl)
        {
            SourceCoverUrl = coverUrl;
            CoverUrl = coverUrl + "@400w_250h_1c_100q.jpg";
        }
    }
}