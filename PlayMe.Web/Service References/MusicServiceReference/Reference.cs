﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlayMe.Web.MusicServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ObjectId", Namespace="http://schemas.datacontract.org/2004/07/MongoDB.Bson")]
    [System.SerializableAttribute()]
    public partial struct ObjectId : System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int _incrementField;
        
        private int _machineField;
        
        private short _pidField;
        
        private int _timestampField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int _increment {
            get {
                return this._incrementField;
            }
            set {
                if ((this._incrementField.Equals(value) != true)) {
                    this._incrementField = value;
                    this.RaisePropertyChanged("_increment");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int _machine {
            get {
                return this._machineField;
            }
            set {
                if ((this._machineField.Equals(value) != true)) {
                    this._machineField = value;
                    this.RaisePropertyChanged("_machine");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public short _pid {
            get {
                return this._pidField;
            }
            set {
                if ((this._pidField.Equals(value) != true)) {
                    this._pidField = value;
                    this.RaisePropertyChanged("_pid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int _timestamp {
            get {
                return this._timestampField;
            }
            set {
                if ((this._timestampField.Equals(value) != true)) {
                    this._timestampField = value;
                    this.RaisePropertyChanged("_timestamp");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MusicServiceReference.IMusicService")]
    public interface IMusicService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetActiveProviders", ReplyAction="http://tempuri.org/IMusicService/GetActiveProvidersResponse")]
        PlayMe.Common.Model.MusicProviderDescriptor[] GetActiveProviders();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetActiveProviders", ReplyAction="http://tempuri.org/IMusicService/GetActiveProvidersResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.MusicProviderDescriptor[]> GetActiveProvidersAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/SearchAll", ReplyAction="http://tempuri.org/IMusicService/SearchAllResponse")]
        PlayMe.Common.Model.SearchResults SearchAll(string searchTerm, string provider, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/SearchAll", ReplyAction="http://tempuri.org/IMusicService/SearchAllResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.SearchResults> SearchAllAsync(string searchTerm, string provider, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/MatchSearchTermHistory", ReplyAction="http://tempuri.org/IMusicService/MatchSearchTermHistoryResponse")]
        string[] MatchSearchTermHistory(string partialSearchTerm);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/MatchSearchTermHistory", ReplyAction="http://tempuri.org/IMusicService/MatchSearchTermHistoryResponse")]
        System.Threading.Tasks.Task<string[]> MatchSearchTermHistoryAsync(string partialSearchTerm);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/BrowseArtist", ReplyAction="http://tempuri.org/IMusicService/BrowseArtistResponse")]
        PlayMe.Common.Model.Artist BrowseArtist(string link, string provider);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/BrowseArtist", ReplyAction="http://tempuri.org/IMusicService/BrowseArtistResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.Artist> BrowseArtistAsync(string link, string provider);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/BrowseAlbum", ReplyAction="http://tempuri.org/IMusicService/BrowseAlbumResponse")]
        PlayMe.Common.Model.Album BrowseAlbum(string link, string provider, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/BrowseAlbum", ReplyAction="http://tempuri.org/IMusicService/BrowseAlbumResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.Album> BrowseAlbumAsync(string link, string provider, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetTrack", ReplyAction="http://tempuri.org/IMusicService/GetTrackResponse")]
        PlayMe.Common.Model.Track GetTrack(string link, string provider, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetTrack", ReplyAction="http://tempuri.org/IMusicService/GetTrackResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.Track> GetTrackAsync(string link, string provider, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/QueueTrack", ReplyAction="http://tempuri.org/IMusicService/QueueTrackResponse")]
        string QueueTrack(PlayMe.Common.Model.QueuedTrack queuedTrack);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/QueueTrack", ReplyAction="http://tempuri.org/IMusicService/QueueTrackResponse")]
        System.Threading.Tasks.Task<string> QueueTrackAsync(PlayMe.Common.Model.QueuedTrack queuedTrack);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/VetoTrack", ReplyAction="http://tempuri.org/IMusicService/VetoTrackResponse")]
        void VetoTrack(System.Guid queuedTrackId, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/VetoTrack", ReplyAction="http://tempuri.org/IMusicService/VetoTrackResponse")]
        System.Threading.Tasks.Task VetoTrackAsync(System.Guid queuedTrackId, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetQueue", ReplyAction="http://tempuri.org/IMusicService/GetQueueResponse")]
        PlayMe.Common.Model.QueuedTrack[] GetQueue();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetQueue", ReplyAction="http://tempuri.org/IMusicService/GetQueueResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.QueuedTrack[]> GetQueueAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/ForgetTrack", ReplyAction="http://tempuri.org/IMusicService/ForgetTrackResponse")]
        void ForgetTrack(System.Guid queuedTrackId, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/ForgetTrack", ReplyAction="http://tempuri.org/IMusicService/ForgetTrackResponse")]
        System.Threading.Tasks.Task ForgetTrackAsync(System.Guid queuedTrackId, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/SkipTrack", ReplyAction="http://tempuri.org/IMusicService/SkipTrackResponse")]
        void SkipTrack(System.Guid queuedTrackId, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/SkipTrack", ReplyAction="http://tempuri.org/IMusicService/SkipTrackResponse")]
        System.Threading.Tasks.Task SkipTrackAsync(System.Guid queuedTrackId, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetPlayingTrack", ReplyAction="http://tempuri.org/IMusicService/GetPlayingTrackResponse")]
        PlayMe.Common.Model.QueuedTrack GetPlayingTrack();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetPlayingTrack", ReplyAction="http://tempuri.org/IMusicService/GetPlayingTrackResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.QueuedTrack> GetPlayingTrackAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/PauseTrack", ReplyAction="http://tempuri.org/IMusicService/PauseTrackResponse")]
        void PauseTrack(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/PauseTrack", ReplyAction="http://tempuri.org/IMusicService/PauseTrackResponse")]
        System.Threading.Tasks.Task PauseTrackAsync(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/ResumeTrack", ReplyAction="http://tempuri.org/IMusicService/ResumeTrackResponse")]
        void ResumeTrack(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/ResumeTrack", ReplyAction="http://tempuri.org/IMusicService/ResumeTrackResponse")]
        System.Threading.Tasks.Task ResumeTrackAsync(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetCurrentVolume", ReplyAction="http://tempuri.org/IMusicService/GetCurrentVolumeResponse")]
        int GetCurrentVolume();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetCurrentVolume", ReplyAction="http://tempuri.org/IMusicService/GetCurrentVolumeResponse")]
        System.Threading.Tasks.Task<int> GetCurrentVolumeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/IncreaseVolume", ReplyAction="http://tempuri.org/IMusicService/IncreaseVolumeResponse")]
        void IncreaseVolume(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/IncreaseVolume", ReplyAction="http://tempuri.org/IMusicService/IncreaseVolumeResponse")]
        System.Threading.Tasks.Task IncreaseVolumeAsync(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/DecreaseVolume", ReplyAction="http://tempuri.org/IMusicService/DecreaseVolumeResponse")]
        void DecreaseVolume(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/DecreaseVolume", ReplyAction="http://tempuri.org/IMusicService/DecreaseVolumeResponse")]
        System.Threading.Tasks.Task DecreaseVolumeAsync(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetTrackHistory", ReplyAction="http://tempuri.org/IMusicService/GetTrackHistoryResponse")]
        PlayMe.Common.Model.PagedResult<PlayMe.Common.Model.QueuedTrack> GetTrackHistory(int start, int limit, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetTrackHistory", ReplyAction="http://tempuri.org/IMusicService/GetTrackHistoryResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.PagedResult<PlayMe.Common.Model.QueuedTrack>> GetTrackHistoryAsync(int start, int limit, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetLikes", ReplyAction="http://tempuri.org/IMusicService/GetLikesResponse")]
        PlayMe.Common.Model.PagedResult<PlayMe.Common.Model.Track> GetLikes(int start, int limit, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetLikes", ReplyAction="http://tempuri.org/IMusicService/GetLikesResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.PagedResult<PlayMe.Common.Model.Track>> GetLikesAsync(int start, int limit, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/LikeTrack", ReplyAction="http://tempuri.org/IMusicService/LikeTrackResponse")]
        void LikeTrack(System.Guid queuedTrackId, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/LikeTrack", ReplyAction="http://tempuri.org/IMusicService/LikeTrackResponse")]
        System.Threading.Tasks.Task LikeTrackAsync(System.Guid queuedTrackId, string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetAllRickRolls", ReplyAction="http://tempuri.org/IMusicService/GetAllRickRollsResponse")]
        PlayMe.Common.Model.RickRoll[] GetAllRickRolls();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetAllRickRolls", ReplyAction="http://tempuri.org/IMusicService/GetAllRickRollsResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.RickRoll[]> GetAllRickRollsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/AddRickRoll", ReplyAction="http://tempuri.org/IMusicService/AddRickRollResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(PlayMe.Common.Model.Artist))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(PlayMe.Common.Model.Track))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(PlayMe.Common.Model.Album))]
        PlayMe.Common.Model.RickRoll AddRickRoll(PlayMe.Common.Model.PlayMeObject playMeObject);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/AddRickRoll", ReplyAction="http://tempuri.org/IMusicService/AddRickRollResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.RickRoll> AddRickRollAsync(PlayMe.Common.Model.PlayMeObject playMeObject);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/RemoveRickRoll", ReplyAction="http://tempuri.org/IMusicService/RemoveRickRollResponse")]
        void RemoveRickRoll(System.Guid id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/RemoveRickRoll", ReplyAction="http://tempuri.org/IMusicService/RemoveRickRollResponse")]
        System.Threading.Tasks.Task RemoveRickRollAsync(System.Guid id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/IsUserAdmin", ReplyAction="http://tempuri.org/IMusicService/IsUserAdminResponse")]
        bool IsUserAdmin(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/IsUserAdmin", ReplyAction="http://tempuri.org/IMusicService/IsUserAdminResponse")]
        System.Threading.Tasks.Task<bool> IsUserAdminAsync(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/IsUserSuperAdmin", ReplyAction="http://tempuri.org/IMusicService/IsUserSuperAdminResponse")]
        bool IsUserSuperAdmin(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/IsUserSuperAdmin", ReplyAction="http://tempuri.org/IMusicService/IsUserSuperAdminResponse")]
        System.Threading.Tasks.Task<bool> IsUserSuperAdminAsync(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetAdminUsers", ReplyAction="http://tempuri.org/IMusicService/GetAdminUsersResponse")]
        PlayMe.Common.Model.User[] GetAdminUsers();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetAdminUsers", ReplyAction="http://tempuri.org/IMusicService/GetAdminUsersResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.User[]> GetAdminUsersAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/AddAdminUser", ReplyAction="http://tempuri.org/IMusicService/AddAdminUserResponse")]
        PlayMe.Common.Model.User AddAdminUser(PlayMe.Common.Model.User toAdd, string addedBy);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/AddAdminUser", ReplyAction="http://tempuri.org/IMusicService/AddAdminUserResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.User> AddAdminUserAsync(PlayMe.Common.Model.User toAdd, string addedBy);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/RemoveAdminUser", ReplyAction="http://tempuri.org/IMusicService/RemoveAdminUserResponse")]
        void RemoveAdminUser(string username, string removedBy);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/RemoveAdminUser", ReplyAction="http://tempuri.org/IMusicService/RemoveAdminUserResponse")]
        System.Threading.Tasks.Task RemoveAdminUserAsync(string username, string removedBy);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetLogEntries", ReplyAction="http://tempuri.org/IMusicService/GetLogEntriesResponse")]
        PlayMe.Common.Model.PagedResult<PlayMe.Common.Model.LogEntry> GetLogEntries(PlayMe.Common.Model.SortDirection direction, int start, int take);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetLogEntries", ReplyAction="http://tempuri.org/IMusicService/GetLogEntriesResponse")]
        System.Threading.Tasks.Task<PlayMe.Common.Model.PagedResult<PlayMe.Common.Model.LogEntry>> GetLogEntriesAsync(PlayMe.Common.Model.SortDirection direction, int start, int take);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetDomain", ReplyAction="http://tempuri.org/IMusicService/GetDomainResponse")]
        string GetDomain();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMusicService/GetDomain", ReplyAction="http://tempuri.org/IMusicService/GetDomainResponse")]
        System.Threading.Tasks.Task<string> GetDomainAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMusicServiceChannel : PlayMe.Web.MusicServiceReference.IMusicService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MusicServiceClient : System.ServiceModel.ClientBase<PlayMe.Web.MusicServiceReference.IMusicService>, PlayMe.Web.MusicServiceReference.IMusicService {
        
        public MusicServiceClient() {
        }
        
        public MusicServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MusicServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MusicServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MusicServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public PlayMe.Common.Model.MusicProviderDescriptor[] GetActiveProviders() {
            return base.Channel.GetActiveProviders();
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.MusicProviderDescriptor[]> GetActiveProvidersAsync() {
            return base.Channel.GetActiveProvidersAsync();
        }
        
        public PlayMe.Common.Model.SearchResults SearchAll(string searchTerm, string provider, string user) {
            return base.Channel.SearchAll(searchTerm, provider, user);
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.SearchResults> SearchAllAsync(string searchTerm, string provider, string user) {
            return base.Channel.SearchAllAsync(searchTerm, provider, user);
        }
        
        public string[] MatchSearchTermHistory(string partialSearchTerm) {
            return base.Channel.MatchSearchTermHistory(partialSearchTerm);
        }
        
        public System.Threading.Tasks.Task<string[]> MatchSearchTermHistoryAsync(string partialSearchTerm) {
            return base.Channel.MatchSearchTermHistoryAsync(partialSearchTerm);
        }
        
        public PlayMe.Common.Model.Artist BrowseArtist(string link, string provider) {
            return base.Channel.BrowseArtist(link, provider);
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.Artist> BrowseArtistAsync(string link, string provider) {
            return base.Channel.BrowseArtistAsync(link, provider);
        }
        
        public PlayMe.Common.Model.Album BrowseAlbum(string link, string provider, string user) {
            return base.Channel.BrowseAlbum(link, provider, user);
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.Album> BrowseAlbumAsync(string link, string provider, string user) {
            return base.Channel.BrowseAlbumAsync(link, provider, user);
        }
        
        public PlayMe.Common.Model.Track GetTrack(string link, string provider, string user) {
            return base.Channel.GetTrack(link, provider, user);
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.Track> GetTrackAsync(string link, string provider, string user) {
            return base.Channel.GetTrackAsync(link, provider, user);
        }
        
        public string QueueTrack(PlayMe.Common.Model.QueuedTrack queuedTrack) {
            return base.Channel.QueueTrack(queuedTrack);
        }
        
        public System.Threading.Tasks.Task<string> QueueTrackAsync(PlayMe.Common.Model.QueuedTrack queuedTrack) {
            return base.Channel.QueueTrackAsync(queuedTrack);
        }
        
        public void VetoTrack(System.Guid queuedTrackId, string user) {
            base.Channel.VetoTrack(queuedTrackId, user);
        }
        
        public System.Threading.Tasks.Task VetoTrackAsync(System.Guid queuedTrackId, string user) {
            return base.Channel.VetoTrackAsync(queuedTrackId, user);
        }
        
        public PlayMe.Common.Model.QueuedTrack[] GetQueue() {
            return base.Channel.GetQueue();
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.QueuedTrack[]> GetQueueAsync() {
            return base.Channel.GetQueueAsync();
        }
        
        public void ForgetTrack(System.Guid queuedTrackId, string user) {
            base.Channel.ForgetTrack(queuedTrackId, user);
        }
        
        public System.Threading.Tasks.Task ForgetTrackAsync(System.Guid queuedTrackId, string user) {
            return base.Channel.ForgetTrackAsync(queuedTrackId, user);
        }
        
        public void SkipTrack(System.Guid queuedTrackId, string user) {
            base.Channel.SkipTrack(queuedTrackId, user);
        }
        
        public System.Threading.Tasks.Task SkipTrackAsync(System.Guid queuedTrackId, string user) {
            return base.Channel.SkipTrackAsync(queuedTrackId, user);
        }
        
        public PlayMe.Common.Model.QueuedTrack GetPlayingTrack() {
            return base.Channel.GetPlayingTrack();
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.QueuedTrack> GetPlayingTrackAsync() {
            return base.Channel.GetPlayingTrackAsync();
        }
        
        public void PauseTrack(string user) {
            base.Channel.PauseTrack(user);
        }
        
        public System.Threading.Tasks.Task PauseTrackAsync(string user) {
            return base.Channel.PauseTrackAsync(user);
        }
        
        public void ResumeTrack(string user) {
            base.Channel.ResumeTrack(user);
        }
        
        public System.Threading.Tasks.Task ResumeTrackAsync(string user) {
            return base.Channel.ResumeTrackAsync(user);
        }
        
        public int GetCurrentVolume() {
            return base.Channel.GetCurrentVolume();
        }
        
        public System.Threading.Tasks.Task<int> GetCurrentVolumeAsync() {
            return base.Channel.GetCurrentVolumeAsync();
        }
        
        public void IncreaseVolume(string user) {
            base.Channel.IncreaseVolume(user);
        }
        
        public System.Threading.Tasks.Task IncreaseVolumeAsync(string user) {
            return base.Channel.IncreaseVolumeAsync(user);
        }
        
        public void DecreaseVolume(string user) {
            base.Channel.DecreaseVolume(user);
        }
        
        public System.Threading.Tasks.Task DecreaseVolumeAsync(string user) {
            return base.Channel.DecreaseVolumeAsync(user);
        }
        
        public PlayMe.Common.Model.PagedResult<PlayMe.Common.Model.QueuedTrack> GetTrackHistory(int start, int limit, string user) {
            return base.Channel.GetTrackHistory(start, limit, user);
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.PagedResult<PlayMe.Common.Model.QueuedTrack>> GetTrackHistoryAsync(int start, int limit, string user) {
            return base.Channel.GetTrackHistoryAsync(start, limit, user);
        }
        
        public PlayMe.Common.Model.PagedResult<PlayMe.Common.Model.Track> GetLikes(int start, int limit, string user) {
            return base.Channel.GetLikes(start, limit, user);
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.PagedResult<PlayMe.Common.Model.Track>> GetLikesAsync(int start, int limit, string user) {
            return base.Channel.GetLikesAsync(start, limit, user);
        }
        
        public void LikeTrack(System.Guid queuedTrackId, string user) {
            base.Channel.LikeTrack(queuedTrackId, user);
        }
        
        public System.Threading.Tasks.Task LikeTrackAsync(System.Guid queuedTrackId, string user) {
            return base.Channel.LikeTrackAsync(queuedTrackId, user);
        }
        
        public PlayMe.Common.Model.RickRoll[] GetAllRickRolls() {
            return base.Channel.GetAllRickRolls();
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.RickRoll[]> GetAllRickRollsAsync() {
            return base.Channel.GetAllRickRollsAsync();
        }
        
        public PlayMe.Common.Model.RickRoll AddRickRoll(PlayMe.Common.Model.PlayMeObject playMeObject) {
            return base.Channel.AddRickRoll(playMeObject);
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.RickRoll> AddRickRollAsync(PlayMe.Common.Model.PlayMeObject playMeObject) {
            return base.Channel.AddRickRollAsync(playMeObject);
        }
        
        public void RemoveRickRoll(System.Guid id) {
            base.Channel.RemoveRickRoll(id);
        }
        
        public System.Threading.Tasks.Task RemoveRickRollAsync(System.Guid id) {
            return base.Channel.RemoveRickRollAsync(id);
        }
        
        public bool IsUserAdmin(string user) {
            return base.Channel.IsUserAdmin(user);
        }
        
        public System.Threading.Tasks.Task<bool> IsUserAdminAsync(string user) {
            return base.Channel.IsUserAdminAsync(user);
        }
        
        public bool IsUserSuperAdmin(string user) {
            return base.Channel.IsUserSuperAdmin(user);
        }
        
        public System.Threading.Tasks.Task<bool> IsUserSuperAdminAsync(string user) {
            return base.Channel.IsUserSuperAdminAsync(user);
        }
        
        public PlayMe.Common.Model.User[] GetAdminUsers() {
            return base.Channel.GetAdminUsers();
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.User[]> GetAdminUsersAsync() {
            return base.Channel.GetAdminUsersAsync();
        }
        
        public PlayMe.Common.Model.User AddAdminUser(PlayMe.Common.Model.User toAdd, string addedBy) {
            return base.Channel.AddAdminUser(toAdd, addedBy);
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.User> AddAdminUserAsync(PlayMe.Common.Model.User toAdd, string addedBy) {
            return base.Channel.AddAdminUserAsync(toAdd, addedBy);
        }
        
        public void RemoveAdminUser(string username, string removedBy) {
            base.Channel.RemoveAdminUser(username, removedBy);
        }
        
        public System.Threading.Tasks.Task RemoveAdminUserAsync(string username, string removedBy) {
            return base.Channel.RemoveAdminUserAsync(username, removedBy);
        }
        
        public PlayMe.Common.Model.PagedResult<PlayMe.Common.Model.LogEntry> GetLogEntries(PlayMe.Common.Model.SortDirection direction, int start, int take) {
            return base.Channel.GetLogEntries(direction, start, take);
        }
        
        public System.Threading.Tasks.Task<PlayMe.Common.Model.PagedResult<PlayMe.Common.Model.LogEntry>> GetLogEntriesAsync(PlayMe.Common.Model.SortDirection direction, int start, int take) {
            return base.Channel.GetLogEntriesAsync(direction, start, take);
        }
        
        public string GetDomain() {
            return base.Channel.GetDomain();
        }
        
        public System.Threading.Tasks.Task<string> GetDomainAsync() {
            return base.Channel.GetDomainAsync();
        }
    }
}
