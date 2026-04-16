using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.Repositories;

namespace WebEcomerceStoreAPI
{
    public class UnitOfWork
    {
        private readonly StoreDbContext _unitOfWorkcontext;
        private CategoryRepository _categoryRepository;
        private DisCountCodeRepository _DisCountCodeRepository;
        private InventoryRepository _inventoryRepository;
        private OrderDetailRepository _orderDetailRepository;
        private ProductRepository _productRepository;
        private OrderRepository _orderRepository;
        private ReviewsRepository _reviewsRepository;
        private UserRepository _userRepository;
        private ProductImagesRepository _productImagesRepository;
        private PaymentRepository _paymentRepository;
        private RoleRepository _roleRepository;
        public UnitOfWork()
        {
            _unitOfWorkcontext = new StoreDbContext();
        }

        public CategoryRepository Category
        {
            get { return _categoryRepository ??= new CategoryRepository(_unitOfWorkcontext); }
        }

        public DisCountCodeRepository DisCountCode
        {
            get { return _DisCountCodeRepository ??= new DisCountCodeRepository(_unitOfWorkcontext); }
        }

        public ProductRepository Product
        {
            get { return _productRepository ??= new ProductRepository(_unitOfWorkcontext); }
        }
        public ProductImagesRepository ProductImages
        {
            get { return _productImagesRepository ??= new ProductImagesRepository(_unitOfWorkcontext); }
        }
        public InventoryRepository Inventory
        {
            get { return _inventoryRepository ??= new InventoryRepository(_unitOfWorkcontext); }
        }
        public OrderRepository Order
        {
            get { return _orderRepository ??= new OrderRepository(_unitOfWorkcontext); }
        }
        public OrderDetailRepository OrderDetail
        {
            get { return _orderDetailRepository ??= new OrderDetailRepository(_unitOfWorkcontext); }
        }
        public ReviewsRepository Reviews
        {
            get { return _reviewsRepository ??= new ReviewsRepository(_unitOfWorkcontext); }
        }
        public PaymentRepository Payment
        {
            get { return _paymentRepository ??= new PaymentRepository(_unitOfWorkcontext); }
        }
        public RoleRepository Role
        {
            get { return _roleRepository ??= new RoleRepository(_unitOfWorkcontext); }
        }
        public UserRepository User
        {
            get { return _userRepository ??= new UserRepository(_unitOfWorkcontext); }
        }
    }
}
