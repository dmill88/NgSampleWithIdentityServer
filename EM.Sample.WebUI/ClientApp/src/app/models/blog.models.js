"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var BlogStatuses;
(function (BlogStatuses) {
    BlogStatuses[BlogStatuses["Draft"] = 1] = "Draft";
    BlogStatuses[BlogStatuses["Review"] = 2] = "Review";
    BlogStatuses[BlogStatuses["Published"] = 3] = "Published";
    BlogStatuses[BlogStatuses["Archived"] = 4] = "Archived";
})(BlogStatuses = exports.BlogStatuses || (exports.BlogStatuses = {}));
var PostStatuses;
(function (PostStatuses) {
    PostStatuses[PostStatuses["Draft"] = 1] = "Draft";
    PostStatuses[PostStatuses["Review"] = 2] = "Review";
    PostStatuses[PostStatuses["Published"] = 3] = "Published";
    PostStatuses[PostStatuses["Archived"] = 4] = "Archived";
})(PostStatuses = exports.PostStatuses || (exports.PostStatuses = {}));
var CommentStatuses;
(function (CommentStatuses) {
    CommentStatuses[CommentStatuses["MemberOnly"] = 1] = "MemberOnly";
    CommentStatuses[CommentStatuses["Locked"] = 2] = "Locked";
    CommentStatuses[CommentStatuses["Disabled"] = 3] = "Disabled";
    CommentStatuses[CommentStatuses["Anonymous"] = 4] = "Anonymous";
})(CommentStatuses = exports.CommentStatuses || (exports.CommentStatuses = {}));
var Author = /** @class */ (function () {
    function Author() {
    }
    return Author;
}());
exports.Author = Author;
var BlogListItem = /** @class */ (function () {
    function BlogListItem() {
        this.id = 0;
        this.guid = '';
    }
    return BlogListItem;
}());
exports.BlogListItem = BlogListItem;
var Blog = /** @class */ (function () {
    function Blog() {
        this.id = 0;
        this.guid = '';
        this.displayOrder = 1;
        this.blogStatusId = BlogStatuses.Draft;
        this.blogStatus = BlogStatuses.Draft;
    }
    return Blog;
}());
exports.Blog = Blog;
var Post = /** @class */ (function () {
    function Post() {
        this.postStatusId = PostStatuses.Draft;
        this.commentStatusId = CommentStatuses.MemberOnly;
        this.commentCount = 0;
        this.tags = new Array();
    }
    return Post;
}());
exports.Post = Post;
var PostListItem = /** @class */ (function () {
    function PostListItem() {
        this.postStatusId = PostStatuses.Draft;
        this.commentStatusId = CommentStatuses.MemberOnly;
        this.commentCount = 0;
    }
    return PostListItem;
}());
exports.PostListItem = PostListItem;
var BlogPost = /** @class */ (function (_super) {
    __extends(BlogPost, _super);
    function BlogPost() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return BlogPost;
}(Post));
exports.BlogPost = BlogPost;
//# sourceMappingURL=blog.models.js.map